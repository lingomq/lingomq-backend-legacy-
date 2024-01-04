using System.Data;
using Finances.BusinessLayer.Contracts;
using Finances.DomainLayer.Entities;

namespace Finances.BusinessLayer.Services.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private static readonly string Get =
        "SELECT id as \"Id\", email as \"Email\", phone as \"Phone\" " +
        "FROM users";
    private static readonly string GetRange = Get +
                                              " LIMIT @Count";
    private static readonly string GetById = Get +
                                             " WHERE id = @Id";
    private static readonly string Create =
        "INSERT INTO users (id, email, phone) " +
        "VALUES (@Id, @Email, @Phone)";
    private static readonly string Delete =
        "DELETE FROM users " +
        "WHERE id = @Id";
    private static readonly string Update =
        "UPDATE users " +
        "SET " +
        "email = @Email," +
        "phone = @Phone " +
        "WHERE id = @Id";
    public UserRepository(IDbConnection connection) : base(connection) { }

    public async Task<User?> GetAsync(Guid id) =>
        await QuerySingleAsync(GetById, new { Id = id });

    public async Task<List<User>> GetAsync(int range) =>
        await QueryAsync(GetRange, new { Count = range });

    public async Task CreateAsync(User entity) =>
        await ExecuteAsync(Create, entity);

    public async Task UpdateAsync(User entity) =>
        await ExecuteAsync(Update, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteAsync(Delete, new { Id = id });
}