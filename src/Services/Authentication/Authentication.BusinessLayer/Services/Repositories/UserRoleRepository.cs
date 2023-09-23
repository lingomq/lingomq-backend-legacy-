using Authentication.BusinessLayer.Contracts;
using Authentication.DomainLayer.Entities;
using Dapper;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace Authentication.BusinessLayer.Services.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private IDbConnection _connection;
        public UserRoleRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<UserRole> AddAsync(UserRole entity)
        {
            const string sql = "INSERT INTO user_role " +
                               "(id, name) " +
                               "VALUES " +
                               "(@Id, @Name);";

            using var transactionScope = new TransactionScope();

            await _connection.ExecuteAsync(sql, entity);
            transactionScope.Complete();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM user_role " +
                               "WHERE id = @Id";

            using var transactionScope = new TransactionScope();

            await _connection.ExecuteAsync(sql, new { Id = id });
            transactionScope.Complete();

            return true;
        }

        public async Task<List<UserRole>> GetAsync(int count = 0)
        {
            IEnumerable<UserRole> userRoles;

            if (count > 0)
                userRoles = await _connection.QueryAsync<UserRole>(
                    "SELECT * FROM user_role" +
                    "TAKE @Count", new { Count = count });
            else
                userRoles = await _connection.QueryAsync<UserRole>(
                    "SELECT * FROM user_role", new { Count = count });

            return userRoles.ToList();
        }

        public async Task<UserRole?> GetByGuidAsync(Guid guid)
        {
            const string sql = "SELECT * FROM user_role WHERE id = @Id";
            IEnumerable<UserRole> userRoles = await _connection.QueryAsync<UserRole>(sql, new { Id = guid });

            return userRoles.Count() > 0 ? userRoles.First() : null;
        }

        public async Task<UserRole> UpdateAsync(UserRole entity)
        {
            const string sql = "UPDATE user_role" +
                               "SET name = @Name" +
                               "WHERE id = @Id";

            using var transactionScope = new TransactionScope();

            await _connection.ExecuteAsync(sql, entity);
            transactionScope.Complete();

            return entity;
        }
    }
}
