using Achievements.BusinessLayer.Contracts;
using Achievements.DomainLayer.Entities;
using Dapper;
using System.Data;
using System.Transactions;

namespace Achievements.BusinessLayer.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly static string Get =
            "SELECT id as \"Id\", email as \"Email\", phone as \"Phone\", " +
            "password_hash as \"PasswordHash\", password_salt as \"PasswordSalt\" " +
            "FROM users";
        private readonly static string GetRange = Get +
            " LIMIT @Count";
        private readonly static string GetById = Get +
            " WHERE id = @Id";
        private readonly static string GetByEmail = Get +
            " WHERE email = @Email";
        private readonly static string Create =
            "INSERT INTO users (id, email, phone, password_hash, password_salt) " +
            "VALUES (@Id, @Email, @Phone, @PasswordHash, @PasswordSalt)";
        private readonly static string Delete =
            "DELETE FROM users " +
            "WHERE id = @Id";
        private readonly static string Update =
            "UPDATE users " +
            "SET " +
            "email = @Email," +
            "phone = @Phone " +
            "WHERE id = @Id";
        private readonly IDbConnection _connection;
        public UserRepository(IDbConnection configuration) =>
            _connection = configuration;
        public async Task CreateAsync(User entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            int result = await _connection.ExecuteAsync(Create, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            int result = await _connection.ExecuteAsync(Delete, new { Id = id });
            transactionScope.Complete();
            transactionScope.Dispose();
        }

        public async Task<List<User>> GetAsync(int range)
        {
            IEnumerable<User> users;
            users = await _connection.QueryAsync<User>(GetRange, new { Count = range });

            return users.ToList() is null ? new List<User>() : users.ToList();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            IEnumerable<User> users = await _connection.QueryAsync<User>(
                GetByEmail,
                new { Email = email });

            return users.Count() > 0 ? users.First() : null;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            IEnumerable<User> users = await _connection.QueryAsync<User>(
                GetById,
                new { Id = id });

            return users.Count() > 0 ? users.First() : null;
        }

        public async Task UpdateAsync(User entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            int result = await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }
    }
}
