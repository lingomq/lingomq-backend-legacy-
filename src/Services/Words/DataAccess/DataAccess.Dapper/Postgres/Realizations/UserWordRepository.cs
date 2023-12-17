using Dapper;
using DataAccess.Dapper.Contracts;
using DataAccess.Dapper.Postgres.RawQueries;
using DataAccess.Dapper.Utils;
using System.Data;
using Words.Domain.Entities;
using Words.Domain.Enums;
using Words.Domain.Models;

namespace DataAccess.Dapper.Postgres.Realizations;
public class UserWordRepository : GenericRepository<UserWord>, IUserWordRepository
{
    private readonly IDbConnection _connection;
    protected UserWordRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;

    public async Task AddAsync(UserWord entity) =>
        await ExecuteByTemplateAsync(UserWordQueries.Create, entity);

    public async Task AddRepeatsAsync(Guid wordId, int count) =>
        await ExecuteByTemplateAsync(UserWordQueries.AddRepeats, new { WordId = wordId, Repeats = count });

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserWordQueries.Delete, new { Id = id });

    public async Task<List<UserWord>> GetAsync(int range = int.MaxValue) =>
        await QueryListAsync(UserWordQueries.GetRange, new { Count = range });

    public async Task<UserWord?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(UserWordQueries.GetById, new { Id = id });
    
    public async Task<List<UserWord>> GetByUserIdAsync(Guid id) =>
        await QueryListAsync(UserWordQueries.GetByUserId, new { Id = id });

    public async Task<UserWord?> GetByWordAsync(string word) =>
        await QueryFirstAsync(UserWordQueries.GetByWord, new { Word = word });

    public async Task<int> GetCountWordsPerDayAsync(Guid id, DateTime day) =>
        await _connection.QueryFirstAsync(UserWordQueries.GetCountPerDay, new { Id = id, CreatedAt = day });

    public async Task<UserWord?> GetMostRepeatedWordAsync(Guid userId) =>
        await QueryFirstAsync(UserWordQueries.GetMostRepeated, new { UserId = userId });
    
    public async Task<List<RecordsModel>> GetRecordsAsync(RecordTypesEnum recordType, OrderingEnum ordering, int count)
    {
        string sql;

        if (recordType == RecordTypesEnum.Words)
            sql = ordering == OrderingEnum.ASC ? UserWordQueries.GetRecordsByWordsCountAsc : UserWordQueries.GetRecordsByWordsCountDesc;
        else
            sql = ordering == OrderingEnum.ASC ? UserWordQueries.GetRecordsByRepeatsAsc : UserWordQueries.GetRecordsByRepeatsDesc;

        IEnumerable<RecordsModel> records = await _connection.QueryAsync<RecordsModel>(sql, new { Count = count });

        return records.Any() ? records.ToList() : new List<RecordsModel>();
    }

    public async Task UpdateAsync(UserWord entity) =>
        await ExecuteByTemplateAsync(UserWordQueries.Update, entity);

    protected override async Task<List<UserWord>> QueryListAsync<E>(string sql, E entity)
    {
        IEnumerable<UserWord> words;

        words = await _connection.QueryAsync<UserWord, Language, User, UserWord>(
            sql,
            (userWord, language, user) =>
            {
                userWord.LanguageId = language.Id;
                userWord.Language = language;

                userWord.UserId = user.Id;
                userWord.User = user;

                return userWord;
            }, entity, splitOn: "id");

        return words.Any() ? words.ToList() : new List<UserWord>();
    }

    protected override async Task<UserWord?> QueryFirstAsync<E>(string sql, E entity)
    {
        List<UserWord> userWords = await QueryListAsync(sql, entity);
        return userWords.Any() ? userWords.FirstOrDefault() : null;
    }

    Task<List<UserWordType>> IUserWordRepository.GetByUserIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserWordType>> GetByTypeIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(UserWordType entity)
    {
        throw new NotImplementedException();
    }

    Task<List<UserWordType>> IGenericRepository<UserWordType>.GetAsync(int range)
    {
        throw new NotImplementedException();
    }

    Task<UserWordType?> IGenericRepository<UserWordType>.GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UserWordType entity)
    {
        throw new NotImplementedException();
    }
}
