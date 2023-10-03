using Authentication.BusinessLayer.Contracts;
using Authentication.DomainLayer.Entities;
using Dapper;
using System.Data;
using System.Transactions;
using static Dapper.SqlMapper;

namespace Authentication.BusinessLayer.Services.Repositories
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

        private readonly static string UpdateCredential =
            "UPDATE users " +
            "SET " +
            "password_hash = @PasswordHash, " +
            "password_salt = @PasswordSalt " +
            "WHERE id = @Id";
        private readonly IDbConnection _connection;
        public UserRepository(IDbConnection configuration)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connection = configuration;
        }
        public async Task<User> AddAsync(User entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            int result = await _connection.ExecuteAsync(Create, entity).ConfigureAwait(false);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            int result = await _connection.ExecuteAsync(Delete, new { Id = id });
            transactionScope.Complete();
            transactionScope.Dispose();

            return true;
        }

        public async Task<List<User>> GetAsync(int count = 0)
        {
            IEnumerable<User> users;
            if (count > 0)
                users = await _connection.QueryAsync<User>(GetRange,
                    new { Count = count });
            else
                users = await _connection.QueryAsync<User>(Get);

            return users.ToList() is null ? new List<User>() : users.ToList();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            IEnumerable<User> users = await _connection.QueryAsync<User>(
                GetByEmail,
                new { Email = email });

            return users.Count() > 0 ? users.First() : null;
        }

        public async Task<User?> UpdateCredentials(User user)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            int result = await _connection.ExecuteAsync(UpdateCredential, user);
            transactionScope.Complete();
            transactionScope.Dispose();

            return user;
        }

        public async Task<User?> GetByGuidAsync(Guid guid)
        {
            IEnumerable<User> users = await _connection.QueryAsync<User>(
                GetById,
                new { Id = guid });

            return users.Count() > 0? users.First() : null;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            int result = await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }
    }
}
