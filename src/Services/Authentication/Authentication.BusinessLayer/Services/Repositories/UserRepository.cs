using Authentication.BusinessLayer.Contracts;
using Authentication.DomainLayer.Entities;
using Dapper;
using System;
using System.Data;
using System.Transactions;
using static Dapper.SqlMapper;

namespace Authentication.BusinessLayer.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;
        public UserRepository(IDbConnection configuration)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connection = configuration;
        }
        public async Task<User> AddAsync(User entity)
        {
            string sql = 
                "INSERT INTO users (id, email, phone, password_hash, password_salt) " +
                "VALUES (@Id, @Email, @Phone, @PasswordHash, @PasswordSalt)";

            using var transactionScope = new TransactionScope();

            int result = await _connection.ExecuteAsync(sql, entity);
            transactionScope.Complete();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            string sql =
                "DELETE FROM users " +
                "WHERE id = @Id";

            using var transactionScope = new TransactionScope();

            int result = await _connection.ExecuteAsync(sql, new { Id = id });
            transactionScope.Complete();

            return true;
        }

        public async Task<List<User>> GetAsync(int count = 0)
        {
            IEnumerable<User> users;
            if (count > 0)
                users = await _connection.QueryAsync<User>("SELECT * FROM users TAKE @Count;",
                    new { Count = count });
            else
                users = await _connection.QueryAsync<User>("SELECT * FROM users;");

            return users.ToList();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            IEnumerable<User> users = await _connection.QueryAsync<User>(
                "SELECT * FROM users " +
                "WHERE email = @Email", new { Email = email });

            return users.Count() > 0 ? users.First() : null;
        }

        public async Task<User?> GetByGuidAsync(Guid guid)
        {
            IEnumerable<User> users = await _connection.QueryAsync<User>(
                "SELECT * FROM users " +
                "WHERE id = @Id", new { Id = guid });

            return users.Count() > 0? users.First() : null;
        }

        public async Task<User?> GetByNicknameAsync(string nickname)
        {
            IEnumerable<User> users = await _connection.QueryAsync<User>(
                "SELECT u.id u.phone, u.email, u.password_hash, u.password_salt FROM users as u" +
                "INNER JOIN user_infos as i " +
                "WHERE i.nickname = @Nickname", new { Nickname = nickname });

            return users.Count() > 0 ? users.First() : null;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            string sql =
                "UPDATE users" +
                "SET email = @Email," +
                "phone = @Phone," +
                "password_hash = @PasswordHash," +
                "password_salt = @PasswordSalt " +
                "WHERE id = @Id";

            using var transactionScope = new TransactionScope();

            int result = await _connection.ExecuteAsync(sql, entity);
            transactionScope.Complete();

            return entity;
        }
    }
}
