using Dapper;
using System.Data;
using System.Transactions;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Services.Repositories
{
    public class TopicStatisticsTypeRepository : ITopicStatisticsTypeRepository
    {
        private readonly static string Get =
            "SELECT id as \"Id\", " +
            "name as \"TypeName\" " +
            "FROM topic_statistics_types ";
        private readonly static string GetRange = Get +
            "LIMIT @Count";
        private readonly static string GetById = Get +
            "WHERE id = @Id";
        private readonly static string Create =
            "INSERT INTO topic_statistics_types " +
            "(id, name) " +
            "VALUES (@Id, @TypeName)";
        private readonly static string Update =
            "UPDATE topic_statistics_types SET" +
            "name = @TypeName " +
            "WHERE id = @Id";
        private readonly static string Delete =
            "DELETE FROM topic_statistics_types " +
            "WHERE id = @Id";

        private readonly IDbConnection _connection;
        public TopicStatisticsTypeRepository(IDbConnection connection) =>
            _connection = connection;

        public async Task AddAsync(TopicStatisticsType entity)
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

        public async Task<List<TopicStatisticsType>> GetAsync(int range)
        {
            IEnumerable<TopicStatisticsType> types;
            types = await _connection.QueryAsync<TopicStatisticsType>(GetRange, new { Count = range });

            return types.Count() == 0 ? new List<TopicStatisticsType>() : types.ToList();
        }

        public async Task<TopicStatisticsType?> GetByIdAsync(Guid id)
        {
            IEnumerable<TopicStatisticsType> types;
            types = await _connection.QueryAsync<TopicStatisticsType>(GetById, new { Id = id });

            return types.FirstOrDefault();
        }

        public async Task UpdateAsync(TopicStatisticsType entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }
    }
}
