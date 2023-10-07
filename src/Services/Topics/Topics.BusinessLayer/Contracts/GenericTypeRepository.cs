using Dapper;
using System.Data;
using System.Transactions;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Contracts
{
    public abstract class GenericTypeRepository<T> where T : BaseEntity
    {
        private readonly IDbConnection _connection;
        public GenericTypeRepository(IDbConnection connection) =>
            _connection = connection;
        public abstract Task<T?> GetByIdAsync(Guid id);
        public abstract Task<List<T>> GetAsync(int range);
        public abstract Task AddAsync(T entity);
        public abstract Task UpdateAsync(T entity);
        public abstract Task DeleteAsync(Guid id);
        protected virtual async Task<List<T>> GetByQueryAsync<E>(string sql, E entity) where E : class
        {
            IEnumerable<T> values;
            values = await _connection.QueryAsync<T>(sql, entity);

            return values.Count() == 0 ? new List<T>() : values.ToList();
        }
        protected virtual async Task ExecuteByTemplateAsync<E>(string sql, E entity) where E : class
        {
            using var transactionScope = new TransactionScope();

            await _connection.ExecuteAsync(sql, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }
    }
}
