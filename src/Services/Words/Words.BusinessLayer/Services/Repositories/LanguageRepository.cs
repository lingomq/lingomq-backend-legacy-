using Dapper;
using System.Data;
using System.Transactions;
using Words.BusinessLayer.Contracts;
using Words.DomainLayer.Entities;

namespace Words.BusinessLayer.Services.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly static string Get =
            "SELECT id as \"Id\", name as \"Name\" " +
            "FROM languages ";
        private readonly static string GetRange = Get +
            "LIMIT @Count";
        private readonly static string GetById = Get +
            "WHERE id = @Id";
        private readonly static string GetByLanguageName = Get +
            "WHERE name = @Name";
        private readonly static string Create =
            "INSERT INTO languages (id, name) " +
            "VALUES (@Id, @Name)";
        private readonly static string Update =
            "UPDATE languages " +
            "SET name = @Name " +
            "WHERE id = @Id";
        private readonly static string Delete =
            "DELETE FROM languages " +
            "WHERE id = @Id";
        private readonly IDbConnection _connection;
        public LanguageRepository(IDbConnection connection) =>
            _connection = connection;

        public async Task<Language> AddAsync(Language entity)
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

        public async Task<List<Language>> GetAsync(int range = int.MaxValue)
        {
            IEnumerable<Language> languages;

            languages = await _connection.QueryAsync<Language>(GetRange, new { Count = range });

            return languages is null ? new List<Language>() : languages.ToList();
        }

        public async Task<Language?> GetByIdAsync(Guid id)
        {
            IEnumerable<Language> languages;

            languages = await _connection.QueryAsync<Language>(GetById, new { Id = id });

            return languages.FirstOrDefault() is null ? null : languages.FirstOrDefault();
        }

        public async Task<Language?> GetByNameAsync(string name)
        {
            IEnumerable<Language> languages;

            languages = await _connection.QueryAsync<Language>(GetByLanguageName, new { Name = name });

            return languages.FirstOrDefault() is null ? null : languages.FirstOrDefault();
        }

        public async Task<Language> UpdateAsync(Language entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }
    }
}
