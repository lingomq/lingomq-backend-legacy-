using System.Data;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Services.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
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
        public UserRepository(IDbConnection connection) : base(connection) =>
            _connection = connection;

        public async Task AddAsync(User entity)
        {
            await ExecuteByTemplateAsync(Create, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await ExecuteByTemplateAsync(Delete, new { Id = id });
        }

        public async Task<List<User>> GetAsync(int range)
        {
            return await GetByQueryAsync(GetRange, new { Count = range });
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            IEnumerable<User> users = await GetByQueryAsync(GetById, new { Id = id });
            return users.FirstOrDefault();
        }

        public async Task UpdateAsync(User entity)
        {
            await ExecuteByTemplateAsync(Update, entity);
        }
    }
}
