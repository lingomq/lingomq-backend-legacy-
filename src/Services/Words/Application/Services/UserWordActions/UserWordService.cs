using DataAccess.EntityFramework.Contracts;
using DataAccess.EntityFramework.Extensions;
using Words.Application.EventBus.MassTransit;
using Words.Application.Services.WordChecker;
using Words.Domain.Contracts;
using Words.Domain.Entities;
using Words.Domain.Enums;
using Words.Domain.Exceptions.ClientExceptions;
using Words.Domain.Models;

namespace Words.Application.Services.UserWordActions;
public class UserWordService : IUserWordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWordChecker _wordChecker;
    private readonly PublisherBase _publisher;
    public UserWordService(IUnitOfWork unitOfWork, IWordChecker wordChecker, PublisherBase publisher)
    {
        _unitOfWork = unitOfWork;
        _wordChecker = wordChecker;
        _publisher = publisher;
    }
    public async Task AddRepeatsAsync(Guid userId, Guid wordId)
    {
        await _unitOfWork.UserWords.AddRepeatsAsync(userId, wordId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CreateAsync(Guid wordId, Guid userId)
    {
        Word? word = await _unitOfWork.Words.GetByIdAsync(wordId);
        User? user = await _unitOfWork.Users.GetByIdAsync(userId);

        UserWord userWord = new UserWord()
        {
            User = user,
            Word = word,
            AddedAt = DateTime.UtcNow,
            Repeats = 0
        };

        await _unitOfWork.UserWords.AddAsync(userWord);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        if (await _unitOfWork.UserWords.GetByIdAsync(id) is null)
            throw new InvalidDataException<UserWord>(new string[] { "id" });

        await _unitOfWork.UserWords.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> GetAverageUserWordCountsAsync(Guid id, DateTime date)
    {
        int count = await _unitOfWork.UserWords.GetCountWordsPerDayAsync(id, date);
        return count;
    }

    public async Task<UserWord> GetByIdAsync(Guid id)
    {
        UserWord? userWord = await _unitOfWork.UserWords.GetByIdAsync(id);
        if (userWord is null)
            throw new InvalidDataException<UserWord>(new string[] { "id" });

        return userWord;
    }

    public async Task<List<UserWord>> GetByUserIdAsync(Guid id)
    {
        List<UserWord> words = await _unitOfWork.UserWords.GetByUserIdAsync(id);
        return words;
    }

    public async Task<UserWord> GetMostRepeatedAsync(Guid userId)
    {
        UserWord? userWord = await _unitOfWork.UserWords.GetMostRepeatedWordAsync(userId);
        if (userWord is null)
            throw new NotFoundException<UserWord>();

        return userWord;
    }

    public async Task<List<RecordsModel>> GetRecordsAsync(RecordTypesEnum type, OrderingEnum ordering, int count)
    {
        List<RecordsModel> records = await _unitOfWork.UserWords.GetRecordsAsync(type, ordering, count);
        return records;
    }

    // Old logic

    /*private async Task<UserWord> GetWordData(UserWord userWordDto, bool isForce = false, bool isAutocomplete = false)
    {
        Language? language = await _unitOfWork.Languages.GetByIdAsync(userWordDto.LanguageId);
        if (language is null)
            throw new NotFoundException<Language>();

        string[] correctWords = await _wordChecker.SpellCorrector(userWordDto.Word!, language.Name!);

        if (!correctWords.Contains(userWordDto.Word) && !isForce)
            throw new InvalidDataException<WrongWordResponseModel>(new WrongWordResponseModel() 
            {
                RightWords = correctWords,
            }, "wrong word");

        UserWord word = new UserWord()
        {
            Id = Guid.NewGuid(),
            Word = isAutocomplete ? correctWords.First() : userWordDto.Word,
            LanguageId = userWordDto.LanguageId,
            Translated = userWordDto.Translated,
            UserId = userWordDto.UserId,
            CreatedAt = DateTime.Now,
            Repeats = 0
        };

        return word;
    }*/

/*
    private async Task ValidateBeforeExecute(UserWord userWordDto)
    {
        if (await _unitOfWork.Users.GetByIdAsync(userWordDto.User.Id) is null)
            throw new NotFoundException<User>("User doesn't exist");
        if (await _unitOfWork.Languages.GetByIdAsync(userWordDto.LanguageId) is null)
            throw new NotFoundException<Language>("Language doesn't exist");

        if (userWordDto.Word is null)
            throw new InvalidDataException<UserWordType>(parameters: new string[] { "word" });
    }*/


    /* public async Task CreateAsync(UserWord userWord, bool isForce, bool isAutocomplete, Guid userId)
     {
         await ValidateBeforeExecute(userWord);
         UserWord word = await GetWordData(userWord, isForce, isAutocomplete);

         await _publisher.Send(new AppStatisticsCreateOrUpdate()
         {
             TotalWords = 1,
             Date = DateTime.Now
         });
         var words = await _unitOfWork.UserWords.GetByUserIdAsync(userId);
         await _publisher.Send(new CheckAchievements()
         {
             UserId = userWord.UserId,
             WordsCount = words.Count + 1
         });
         await _unitOfWork.UserWords.AddAsync(word);
     }*/
}
