using Dapper;
using System;
using System.Data;
using System.Transactions;
using Words.BusinessLayer.Contracts;
using Words.DomainLayer.Entities;
using static Dapper.SqlMapper;

namespace Words.BusinessLayer.Services.Repositories
{
    public class UserWordTypeRepository : IUserWordTypeRepository
    {
        private readonly static string Get =
            "SELECT " +
            "user_words.id, " +
            "user_words.word as \"Word\", " +
            "user_words.translated as \"Translated\", " +
            "user_words.repeats as \"Repeats\", " +
            "user_words.created_at as \"CreatedAt\", " +
            "languages.id, " +
            "languages.name as \"Name\", " +
            "users.id, " +
            "users.email as \"Email\", " +
            "users.phone as \"Phone\", " +
            "word_types.id, " +
            "word_types.type_name as \"TypeName\" " +
            "FROM user_word_types " +
            "INNER JOIN user_words ON user_words.id = user_word_types.user_word_id " +
            "INNER JOIN languages ON languages.id = user_words.language_id " +
            "INNER JOIN users ON users.id = user_words.user_id " +
            "INNER JOIN word_types ON word_types.id = user_word_types.word_type_id ";
        private readonly static string GetRange = Get +
            "LIMIT @Count";
        private readonly static string GetById = Get +
            "WHERE user_word_types.id = @Id";
        private readonly static string GetByUserId = Get +
            "WHERE users.id = @Id";
        private readonly static string Create =
            "INSERT INTO user_word_types (id, user_word_id, word_type_id) " +
            "VALUES (@Id, @UserWordId, @WordTypeId)";
        private readonly static string Update =
            "UPDATE user_word_types " +
            "SET user_word_id = @UserWordId, word_type_id = @WordTypeId " +
            "WHERE id = @Id";
        private readonly static string Delete =
            "DELETE FROM user_word_types " +
            "WHERE id = @Id";
        private readonly IDbConnection _connection;
        public UserWordTypeRepository(IDbConnection connection) =>
            _connection = connection;
        public async Task<UserWordType> AddAsync(UserWordType entity)
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

        public async Task<List<UserWordType>> GetAsync(int range = int.MaxValue)
        {
            return await TemplateGet(GetRange, new { Count = range });
        }

        public async Task<UserWordType?> GetByIdAsync(Guid id)
        {
            List<UserWordType> types = await TemplateGet(GetById, new { Id = id });

            return types.FirstOrDefault() is null ? null : types.FirstOrDefault();
        }

        public async Task<UserWordType?> GetByUserIdAsync(Guid id)
        {
            List<UserWordType> types = await TemplateGet(GetByUserId, new { Id = id });

            return types.FirstOrDefault() is null ? null : types.FirstOrDefault();
        }

        public async Task<UserWordType> UpdateAsync(UserWordType entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }
        private async Task<List<UserWordType>> TemplateGet<T>(string sql, T entity)
        {
            IEnumerable<UserWordType> words;

            words = await _connection.QueryAsync<UserWordType, UserWord, Language, User, WordType, UserWordType>(
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

            return words is null ? new List<UserWordType>() : words.ToList();
        }
    }
}
