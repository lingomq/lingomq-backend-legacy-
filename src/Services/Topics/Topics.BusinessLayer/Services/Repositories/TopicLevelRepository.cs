using Dapper;
using System.Data;
using System.Transactions;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Services.Repositories
{
    public class TopicLevelRepository : ITopicLevelRepository
    {
        private readonly static string Get =
            "SELECT id as \"Id\", " +
            "name as \"LevelName\" " +
            "FROM topic_levels ";
        private readonly static string GetById = Get +
            "WHERE id = @Id";
        private readonly static string GetRange = Get +
            "LIMIT @Count;";
        private readonly static string Create =
            "INSERT INTO topic_levels " +
            "(id, name) " +
            "VALUES (@Id, @LevelName)";
        private readonly static string Update =
            "UPDATE topic_levels SET " +
            "name = @LevelName " +
            "WHERE id = @Id";
        private readonly static string Delete =
            "DELETE FROM topic_levels " +
            "WHERE id = @Id";

        private readonly IDbConnection _connection;
        public TopicLevelRepository(IDbConnection connection) =>
            _connection = connection;

        public async Task AddAsync(TopicLevel entity)
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

        public async Task<List<TopicLevel>> GetAsync(int range)
        {
            IEnumerable<TopicLevel> levels;
            levels = await _connection.QueryAsync<TopicLevel>(GetRange, new { Count = range });

            return levels.Count() == 0 ? new List<TopicLevel>() : levels.ToList();
        }

        public async Task<TopicLevel?> GetByIdAsync(Guid id)
        {
            IEnumerable<TopicLevel> levels;
            levels = await _connection.QueryAsync<TopicLevel>(GetById, new { Id = id });

            return levels.FirstOrDefault();
        }

        public async Task UpdateAsync(TopicLevel entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }
    }
}
