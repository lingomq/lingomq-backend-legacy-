using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Words.Api.Common;
using Words.BusinessLayer.Contracts;
using Words.BusinessLayer.Dtos;
using Words.BusinessLayer.Exceptions.ClientExceptions;
using Words.DomainLayer.Entities;

namespace Words.Api.Controllers
{
    [Route("api/words/user-words")]
    [ApiController]
    public class UserWordController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWordChecker _wordChecker;
        private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
            .FirstOrDefault()?.Value!);

        public UserWordController(IUnitOfWork unitOfWork, IWordChecker wordChecker)
        {
            _unitOfWork = unitOfWork;
            _wordChecker = wordChecker;
        }

        [HttpGet("user")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get()
        {
            List<UserWord> words = await _unitOfWork.UserWords.GetByUserIdAsync(UserId);
            return LingoMqResponse.OkResult(words);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid userId)
        {
            List<UserWord> words = await _unitOfWork.UserWords.GetByUserIdAsync(userId);
            return LingoMqResponse.OkResult(words);
        }

        [HttpGet("famous")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetMostRepeatedWord()
        {
            UserWord? userWord = await _unitOfWork.UserWords.GetMostRepeatedWordAsync();
            if (userWord is null)
                throw new NotFoundException<UserWord>();

            return LingoMqResponse.OkResult(userWord);
        }

        [HttpGet("word/{wordId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetByWord(string word)
        {
            UserWord? userWord = await _unitOfWork.UserWords.GetByWordAsync(word);
            if (userWord is null)
                throw new NotFoundException<UserWord>();

            return LingoMqResponse.OkResult(userWord);
        }

        [HttpGet("word/count/{userId}&{date}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetCountWordsPerDay(Guid userId, DateTime date)
        {
            int count = await _unitOfWork.UserWords.GetCountWordsPerDayAsync(userId, date);
            return LingoMqResponse.OkResult(new { Count = count });
        }

        [HttpPost("{isForce}&{isAutocomplete}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Create(UserWordDto userWordDto, bool isForce = false, bool isAutocomplete = false)
        {
            await ValidateBeforeExecute(userWordDto);
            UserWord word = await GetWordData(userWordDto, isForce, isAutocomplete);

            await _unitOfWork.UserWords.AddAsync(word);
            return LingoMqResponse.AcceptedResult(word);
        }

        [HttpPut("{isForce}&{isAutocomplete}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Update(UserWordDto userWordDto, bool isForce = false, bool isAutocomplete = false)
        {
            if (await _unitOfWork.UserWords.GetByIdAsync(userWordDto.Id) is null)
                throw new NotFoundException<UserWord>();

            await ValidateBeforeExecute(userWordDto);
            UserWord word = await GetWordData(userWordDto, isForce, isAutocomplete);

            await _unitOfWork.UserWords.UpdateAsync(word);
            return LingoMqResponse.AcceptedResult(word);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _unitOfWork.UserWords.GetByIdAsync(id) is null)
                throw new InvalidDataException<UserWord>(new string[] { "id" });

            await _unitOfWork.UserWords.DeleteAsync(id);
            return LingoMqResponse.AcceptedResult();
        }

        private async Task<UserWord> GetWordData(UserWordDto userWordDto, bool isForce = false, bool isAutocomplete = false)
        {
            string correctWord = await _wordChecker.SpellCorrector(userWordDto.Word!);

            if (!correctWord.Equals(userWordDto) && !isForce)
                throw new InvalidDataException<UserWordType>("wrong word");

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
            if (await _unitOfWork.UserWords.GetByWordAsync(userWordDto.Word) is not null)
                throw new ConflictException<UserWord>("The word is already exist");
        }
    }
}
