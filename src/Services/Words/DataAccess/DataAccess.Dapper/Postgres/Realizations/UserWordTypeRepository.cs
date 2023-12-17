using Dapper;
using DataAccess.Dapper.Contracts;
using DataAccess.Dapper.Postgres.RawQueries;
using DataAccess.Dapper.Utils;
using System.Data;
using Words.Domain.Entities;

namespace DataAccess.Dapper.Postgres.Realizations;
public class UserWordTypeRepository : GenericRepository<UserWordType>, IUserWordTypeRepository
{
    private readonly IDbConnection _connection;
    protected UserWordTypeRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;

    public async Task AddAsync(UserWordType entity) =>
        await ExecuteByTemplateAsync(UserWordTypeQueries.Create, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserWordTypeQueries.Delete, new { Id = id });

    public async Task<List<UserWordType>> GetAsync(int range = int.MaxValue) =>
        await QueryListAsync(UserWordTypeQueries.GetRange, new { Count = range });

    public async Task<UserWordType?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(UserWordTypeQueries.GetById, new { Id = id });

    public async Task<List<UserWordType>> GetByTypeIdAsync(Guid id) =>
        await QueryListAsync(UserWordTypeQueries.GetByTypeId, new { Id = id });

    public async Task<List<UserWordType>> GetByUserIdAsync(Guid id) =>
        await QueryListAsync(UserWordTypeQueries.GetByUserId, new { Id = id });
    
    public async Task UpdateAsync(UserWordType entity) =>
        await ExecuteByTemplateAsync(UserWordTypeQueries.Update, entity);

    protected override async Task<List<UserWordType>> QueryListAsync<E>(string sql, E entity)
    {
        IEnumerable<UserWordType> wordTypes;

        wordTypes = await _connection.QueryAsync<UserWordType, UserWord, Language, User, WordType, UserWordType>(
            sql,
            (userWordType, userWord, language, user, wordType) =>
            {
                userWordType.WordTypeId = wordType.Id;
                userWordType.WordType = wordType;

                userWord.LanguageId = language.Id;
                userWord.Language = language;

                userWord.UserId = user.Id;
                userWord.User = user;

                userWordType.UserWordId = userWord.Id;
                userWordType.UserWord = userWord;

                return userWordType;
            }, entity, splitOn: "id");

        return wordTypes.Any() ? wordTypes.ToList() : new List<UserWordType>();
    }

    protected override async Task<UserWordType?> QueryFirstAsync<E>(string sql, E entity)
    {
        List<UserWordType> wordTypes = await QueryListAsync(sql, entity);
        return wordTypes.Any() ? wordTypes.FirstOrDefault() : null;
    }
}
