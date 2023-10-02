using Authentication.BusinessLayer.Contracts;
using Authentication.DomainLayer.Entities;
using Dapper;
using System.Data;
using System.Transactions;

namespace Authentication.BusinessLayer.Services.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private static readonly string Get =
            "SELECT id as \"Id\", name as \"Name\" " +
            "FROM user_roles";
        private static readonly string GetById = Get +
            " WHERE id = @Id";
        private static readonly string GetByName = Get +
            " WHERE name = @Name";
        private static readonly string GetRange = Get +
            " LIMIT @Count";
        private static readonly string Create =
            "INSERT INTO user_roles " +
            "(id, name) " +
            "VALUES " +
            "(@Id, @Name);";
        private static readonly string Delete =
            "DELETE FROM user_roles " +
            "WHERE id = @Id";
        private static readonly string Update =
            "UPDATE user_roles " +
            "SET name = @Name" +
            "WHERE id = @Id";

        private IDbConnection _connection;
        public UserRoleRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<UserRole> AddAsync(UserRole entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Create, entity);
            transactionScope.Complete();

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

        public async Task<List<UserRole>> GetAsync(int count = 0)
        {
            IEnumerable<UserRole> userRoles;

            userRoles = await _connection.QueryAsync<UserRole>(GetRange, new { Count = count });

            return userRoles.ToList();
        }

        public async Task<UserRole?> GetByGuidAsync(Guid guid)
        {
            IEnumerable<UserRole> userRoles = await _connection.QueryAsync<UserRole>(
                GetById, new { Id = guid });

            return userRoles.Count() > 0 ? userRoles.First() : null;
        }

        public async Task<UserRole?> GetByNameAsync(string name)
        {
            IEnumerable<UserRole> userRoles = await _connection.QueryAsync<UserRole>(GetByName, 
                new { Name = name });

            return userRoles.Count() > 0 ? userRoles.First() : null;
        }

        public async Task<UserRole> UpdateAsync(UserRole entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }
    }
}
