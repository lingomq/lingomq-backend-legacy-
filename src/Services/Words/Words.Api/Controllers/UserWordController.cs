using EventBus.Entities.Achievements;
using EventBus.Entities.AppStatistics;
using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Security.Claims;
using Words.Api.Common;
using Words.BusinessLayer.Contracts;
using Words.BusinessLayer.Dtos;
using Words.BusinessLayer.Exceptions.ClientExceptions;
using Words.BusinessLayer.MassTransit;
using Words.BusinessLayer.Models;
using Words.BusinessLayer.Services;
using Words.DomainLayer.Entities;

namespace Words.Api.Controllers
{
    [Route("api/words/user-words")]
    [ApiController]
    public class UserWordController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWordChecker _wordChecker;
        private readonly PublisherBase _publisher;
        private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
            .FirstOrDefault()?.Value!);

        public UserWordController(IUnitOfWork unitOfWork, IWordChecker wordChecker, PublisherBase publisher)
        {
            _unitOfWork = unitOfWork;
            _wordChecker = wordChecker;
            _publisher = publisher;
        }

        [HttpGet("user")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get()
        {
            List<UserWord> words = await _unitOfWork.UserWords.GetByUserIdAsync(UserId);
            _logger.Info("GET /user {0}", nameof(List<UserWord>));
            return LingoMqResponse.OkResult(words);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid userId)
        {
            List<UserWord> words = await _unitOfWork.UserWords.GetByUserIdAsync(userId);
            _logger.Info("GET /user/{userId} {0}", nameof(List<UserWord>));
            return LingoMqResponse.OkResult(words);
        }

        [HttpGet("famous")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetMostRepeatedWord()
        {
            UserWord? userWord = await _unitOfWork.UserWords.GetMostRepeatedWordAsync(UserId);
            if (userWord is null)
                throw new NotFoundException<UserWord>();

            _logger.Info("GET /famous {0}", nameof(UserWord));
            return LingoMqResponse.OkResult(userWord);
        }

        [HttpGet("records/repeats/order/{order}/count/{count}")]
        public async Task<IActionResult> GetRecordsByRepeats(int count = 4, string order = "ASC")
        {
            List<RecordsByRepeatsResponseModel> records = await _unitOfWork.UserWords.GetRecordsByRepeatsAsync(count, order);

            return LingoMqResponse.OkResult(records);
        }

        [HttpGet("records/word-count/{count}")]
        public async Task<IActionResult> GetRecordsByWordsCount(int count = 4)
        {
            List<RecordsByWordsCountResponseModel> records = await _unitOfWork.UserWords.GetRecordsByWordsCountsAsync(count);

            return LingoMqResponse.OkResult(records);
        }

        [HttpGet("word/{wordId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetByWord(string word)
        {
            UserWord? userWord = await _unitOfWork.UserWords.GetByWordAsync(word);
            if (userWord is null)
                throw new NotFoundException<UserWord>();

            _logger.Info("GET /word/{wordId} {0}", nameof(UserWord));
            return LingoMqResponse.OkResult(userWord);
        }

        [HttpGet("word/count/{userId}/{date}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetCountWordsPerDay(Guid userId, DateTime date)
        {
            int count = await _unitOfWork.UserWords.GetCountWordsPerDayAsync(userId, date);
            _logger.Info("GET /word/count/{userId}&{date} {0}", nameof(StatusCode));
            return LingoMqResponse.OkResult(new { Count = count });
        }

        [HttpPost("{isForce}/{isAutocomplete}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Create([FromBody] UserWordDto userWordDto, bool isForce = false, bool isAutocomplete = false)
        {
            await ValidateBeforeExecute(userWordDto);
            UserWord word = await GetWordData(userWordDto, isForce, isAutocomplete);

            await _publisher.Send(new AppStatisticsCreateOrUpdate()
            {
                TotalWords = 1,
                Date = DateTime.Now
            });
            var words = await _unitOfWork.UserWords.GetByUserIdAsync(UserId);
            await _publisher.Send(new CheckAchievements()
            {
                UserId = userWordDto.UserId,
                WordsCount = words.Count + 1
            });
            await _unitOfWork.UserWords.AddAsync(word);
            _logger.Info("POST /{isForce}&{isAutocomplete} {0}", nameof(UserWord));
            return LingoMqResponse.AcceptedResult(word);
        }

        [HttpPut("add/repeats/{wordId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> AddRepeats(Guid wordId)
        {
            await _unitOfWork.UserWords.AddRepeatsAsync(wordId, 1);
            return LingoMqResponse.AcceptedResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _unitOfWork.UserWords.GetByIdAsync(id) is null)
                throw new InvalidDataException<UserWord>(new string[] { "id" });

            await _unitOfWork.UserWords.DeleteAsync(id);
            _logger.Info("DELETE /{id} {0}", nameof(UserWord));
            return LingoMqResponse.AcceptedResult();
        }

        private async Task<UserWord> GetWordData(UserWordDto userWordDto, bool isForce = false, bool isAutocomplete = false)
        {
            Language? language = await _unitOfWork.Languages.GetByIdAsync(userWordDto.LanguageId);
            if (language is null)
                throw new NotFoundException<Language>();

            string correctWord = await _wordChecker.SpellCorrector(userWordDto.Word!, language.Name!);

            if (!correctWord.Equals(userWordDto.Word) && !isForce)
                throw new InvalidDataException<RightWordModel>(new RightWordModel() { RightWord = correctWord }, "wrong word");

            UserWord word = new UserWord()
            {
                Id = Guid.NewGuid(),
                Word = isAutocomplete ? correctWord : userWordDto.Word,
                LanguageId = userWordDto.LanguageId,
                Translated = userWordDto.Translated,
                UserId = userWordDto.UserId,
                CreatedAt = DateTime.Now,
                Repeats = 0
            };

            return word;
        }
        private async Task ValidateBeforeExecute(UserWordDto userWordDto)
        {
            if (await _unitOfWork.Users.GetByIdAsync(userWordDto.UserId) is null)
                throw new NotFoundException<User>("User doesn't exist");
            if (await _unitOfWork.Languages.GetByIdAsync(userWordDto.LanguageId) is null)
                throw new NotFoundException<Language>("Language doesn't exist");

            if (userWordDto.Word is null)
                throw new InvalidDataException<UserWordType>(parameters: new string[] { "word" });
        }
    }
}
