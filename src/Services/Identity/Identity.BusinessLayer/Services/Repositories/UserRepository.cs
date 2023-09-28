using AutoMapper;
using Dapper;
using Identity.BusinessLayer.Contracts;
using Identity.BusinessLayer.Dtos;
using Identity.DomainLayer.Entities;
using System.Data;
using System.Transactions;
using static Dapper.SqlMapper;

namespace Identity.BusinessLayer.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly static string Get =
            "SELECT id as \"Id\", email as \"Email\", phone as \"Phone\", " +
            "password_hash as \"PasswordHash\", password_salt as \"PasswordSalt\"" +
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
            "UPDATE users" +
            "SET " +
            "email = @Email," +
            "phone = @Phone," +
            "password_hash = @PasswordHash," +
            "password_salt = @PasswordSalt " +
            "WHERE id = @Id";
        private readonly IDbConnection _connection;
        private readonly IMapper _mapper;
        public UserRepository(IDbConnection connection, IMapper mapper)
        {
            _connection = connection;
            _mapper = mapper;
        }
        public async Task<UserDto> AddAsync(User entity)
        {
            using var transactionScope = new TransactionScope();

            await _connection.ExecuteAsync(Create, entity);
            transactionScope.Complete();

            UserDto userDto = _mapper.Map<UserDto>(entity);
            return userDto;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var transactionScope = new TransactionScope();

            await _connection.ExecuteAsync(Delete, new { Id = id });
            transactionScope.Complete();

            return true;
        }

        public async Task<List<UserDto>> GetAsync(int count)
        {
            IEnumerable<User> user;

            user = await _connection.QueryAsync<User>(GetRange, new { Count = count });

            List<UserDto> userDtos = _mapper.Map<List<UserDto>>(user);
            return userDtos;
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            IEnumerable<User> user;

            user = await _connection.QueryAsync<User>(GetByEmail, new { Email = email });

            List<UserDto> userDtos = _mapper.Map<List<UserDto>>(user);
            return userDtos.FirstOrDefault() is null ? null : userDtos.First();
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            IEnumerable<User> user;

            user = await _connection.QueryAsync<User>(GetById, new { Id = id });

            List<UserDto> userDtos = _mapper.Map<List<UserDto>>(user);
            return userDtos.FirstOrDefault() is null ? null : userDtos.First();
        }

        public async Task<UserDto> UpdateAsync(User entity)
        {
            using var transactionScope = new TransactionScope();

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();

            UserDto userDto = _mapper.Map<UserDto>(entity);
            return userDto;
        }
    }
}
