using Dapper;
using System;
using System.Data;
using System.Transactions;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly static string Get =
            "SELECT id as \"Id\", email as \"Email\", phone as \"Phone\" " +
            "FROM users";
        private readonly static string GetRange = Get +
            " LIMIT @Count";
        private readonly static string GetById = Get +
            " WHERE id = @Id";
        private readonly static string Create =
            "INSERT INTO users (id, email, phone) " +
            "VALUES (@Id, @Email, @Phone)";
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
        public UserRepository(IDbConnection connection) =>
            _connection = connection;

        public async Task AddAsync(User entity)
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

        public async Task<List<User>> GetAsync(int range)
        {
            IEnumerable<User> users;
            users = await _connection.QueryAsync<User>(GetRange, new { Count = range });

            return users.Count() == 0 ? new List<User>() : users.ToList();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            IEnumerable<User> users;
            users = await _connection.QueryAsync<User>(GetById, new { Id = id });

            return users.FirstOrDefault();
        }

        public async Task UpdateAsync(User entity)
        {
            using var transactionScope = new TransactionScope();

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }
    }
}
