using Dapper;
using System.Data;
using System.Transactions;
using Words.BusinessLayer.Contracts;
using Words.DomainLayer.Entities;

namespace Words.BusinessLayer.Services.Repositories
{
    public class UserWordRepository : IUserWordRepository
    {
        private readonly static string Get =
            "SELECT user_words.id, " +
            "word as \"Word\", " +
            "translated as \"Translated\", " +
            "repeats as \"Repeats\", " +
            "created_at as \"CreatedAt\", " +
            "languages.id, " +
            "languages.name as \"Name\", " +
            "users.id, " +
            "users.email as \"Email\", " +
            "users.phone as \"Phone\" " +
            "FROM user_words " +
            "INNER JOIN languages ON languages.id = user_words.language_id " +
            "INNER JOIN users ON users.id = user_words.user_id ";
        private readonly static string GetRange = Get +
            "LIMIT @Count";
        private readonly static string GetById = Get +
            "WHERE id = @Id";
        private readonly static string GetByWord = Get +
            "WHERE word = @Word";
        private readonly static string GetCountPerDay =
            "SELECT COUNT(*) FROM user_words " +
            "WHERE id = @Id, created_at = @CreatedAt";
        private readonly static string GetMostRepeated = Get +
            "WHERE repeats = (SELECT MAX(repeats) FROM user_words) ";
        private readonly static string Create =
            "INSERT INTO user_words (id, word, translated, repeats, created_at, language_id, user_id) " +
            "VALUES (@Id, @Word, @Translated, @Repeats, @CreatedAt, @LanguageId, @UserId)";
        private readonly static string Update =
            "UPDATE user_words " +
            "SET word = @Word, translated = @Translated, repeats = @Repeats, " +
            "created_at = @CreatedAt, language_id = @LanguageId, user_id = @UserId";
        private readonly static string Delete =
            "DELETE FROM user_words" +
            "WHERE id = @Id";
        private readonly IDbConnection _connection;
        public UserWordRepository(IDbConnection connection) =>
            _connection = connection;

        public async Task<UserWord> AddAsync(UserWord entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Create, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Delete, new { Id = id });
            transactionScope.Complete();
            transactionScope.Dispose();

            return true;
        }

        public async Task<List<UserWord>> GetAsync(int range = int.MaxValue)
        {
            return await TemplateGet(GetRange, new { Count = range });
        }

        public async Task<UserWord?> GetByIdAsync(Guid id)
        {
            List<UserWord> words = await TemplateGet(GetById, new { Id = id });

            return words.FirstOrDefault() is null ? null : words.FirstOrDefault();
        }

        public async Task<UserWord?> GetByWordAsync(string word)
        {
            List<UserWord> words = await TemplateGet(GetByWord, new { Word = word });

            return words.FirstOrDefault() is null ? null : words.FirstOrDefault();
        }

        public async Task<int> GetCountWordsPerDayAsync(Guid id, DateTime day)
        {
            var result = await _connection.QueryAsync<int>(GetCountPerDay, new { Id = id, CreatedAt = day });

            return result.First();
        }

        public async Task<UserWord?> GetMostRepeatedWordAsync()
        {
            List<UserWord> words = await TemplateGet(GetMostRepeated);

            return words.First();
        }

        public async Task<UserWord> UpdateAsync(UserWord entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }

        private async Task<List<UserWord>> TemplateGet<T>(string sql, T entity)
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

            return words is null ? new List<UserWord>() : words.ToList();
        }

        private async Task<List<UserWord>> TemplateGet(string sql)
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
                }, splitOn: "id");

            return words is null ? new List<UserWord>() : words.ToList();
        }
    }
}
