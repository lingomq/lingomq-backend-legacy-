using System.Data;
using Notifications.BusinessLayer.Contracts;
using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.Services.Repositories;

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
    
    public UserRepository(IDbConnection connection) : base(connection) { }

    public async Task<User?> GetAsync(Guid id) => await QueryFirstAsync(GetById, new { Id = id });

    public async Task<List<User>> GetAsync(int range) => await QueryAsync(GetRange, new { Count = range });

    public async Task UpdateAsync(User entity) => await ExecuteAsync(Update, entity);

    public async Task DeleteAsync(Guid id) => await ExecuteAsync(Delete, new { Id = id });

    public async Task CreateAsync(User entity) => await ExecuteAsync(Create, entity);
}