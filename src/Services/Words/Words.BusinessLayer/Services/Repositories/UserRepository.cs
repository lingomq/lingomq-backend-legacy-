using Dapper;
using System.Data;
using System.Transactions;
using Words.BusinessLayer.Contracts;
using Words.BusinessLayer.Dtos;

namespace Words.BusinessLayer.Services.Repositories
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
        private readonly static string GetByEmail = Get +
            " WHERE email = @Email";
        private readonly static string Create =
            "INSERT INTO users (id, email, phone, password_hash, password_salt) " +
            "VALUES (@Id, @Email, @Phone, '', '')";
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

        public async Task<UserDto> AddAsync(UserDto entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Create, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

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

        public async Task<List<UserDto>> GetAsync(int range = int.MaxValue)
        {
            IEnumerable<UserDto> users;
            
            users = await _connection.QueryAsync<UserDto>(GetRange, new { Count = range });

            return users is null ? new List<UserDto>() : users.ToList();
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            IEnumerable<UserDto> users;

            users = await _connection.QueryAsync<UserDto>(GetById, new { Id = id });

            return users.FirstOrDefault() is null ? null : users.FirstOrDefault();
        }

        public async Task<UserDto> UpdateAsync(UserDto entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }
    }
}
