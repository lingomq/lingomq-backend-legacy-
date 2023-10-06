using Achievements.BusinessLayer.Contracts;
using Achievements.DomainLayer.Entities;
using System.Data;
using Dapper;
using System.Transactions;

namespace Achievements.BusinessLayer.Services.Repositories
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly static string Get =
            "SELECT id as \"Id\"," +
            "name as \"Name\"," +
            "content as \"Content\"," +
            "image_uri as \"ImageUri\" " +
            "FROM achievements ";
        private readonly static string GetRange = Get +
            "LIMIT @Count";
        private readonly static string GetById = Get +
            "WHERE id = @Id";
        private readonly static string Create =
            "INSERT INTO achievements " +
            "(id, name, content, image_uri) " +
            "VALUES (@Id, @Name, @Content, @ImageUri)";
        private readonly static string Update =
            "UPDATE achievements SET" +
            "name = @Name, " +
            "content = @Content, " +
            "image_uri = @ImageUri " +
            "WHERE id = @Id";
        private readonly static string Delete =
            "DELETE FROM achievements " +
            "WHERE id = @Id";

        private readonly IDbConnection _connection;
        public AchievementRepository(IDbConnection connection) =>
            _connection = connection;
        public async Task CreateAsync(Achievement entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Create, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Delete, new { Id = id });
            transactionScope.Complete();
            transactionScope.Dispose();
        }

        public async Task<List<Achievement>> GetAsync(int range)
        {
            IEnumerable<Achievement>? achievements;
            achievements = await _connection.QueryAsync<Achievement>(GetRange, new { Count = range });

            return achievements.Count() == 0 ? new List<Achievement>() : achievements.ToList();
        }

        public async Task<Achievement?> GetByIdAsync(Guid id)
        {
            IEnumerable<Achievement>? achievements;
            achievements = await _connection.QueryAsync<Achievement>(GetById, new { Id = id });

            return achievements.FirstOrDefault() is null ? null: achievements.FirstOrDefault();
        }

        public async Task UpdateAsync(Achievement entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }
    }
}
