using Dapper;
using System.Data;
using System.Transactions;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Contracts
{
    public abstract class GenericRepository<T> where T : BaseEntity
    {
        private readonly IDbConnection _connection;
        protected GenericRepository(IDbConnection connection) =>
            _connection = connection;
        protected virtual async Task<List<T>> GetByQueryAsync<E>(string sql, E entity) where E : class
        {
            IEnumerable<T> values;
            values = await _connection.QueryAsync<T>(sql, entity);

            return values.Count() == 0 ? new List<T>() : values.ToList();
        }
        protected virtual async Task ExecuteByTemplateAsync<TE>(string sql, TE entity) where TE : class
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(sql, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }
    }
}
