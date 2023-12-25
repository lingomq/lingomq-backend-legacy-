using DataAccess.Dapper.Contracts;
using DataAccess.Dapper.Postgres.RawQueries;
using DataAccess.Dapper.Utils;
using Domain.Dtos;
using Identity.Domain.Entities;
using System.Data;

namespace DataAccess.Dapper.Postgres.Realizations;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly IDbConnection _connection;
    public UserRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;
    public async Task AddAsync(User entity) =>
        await ExecuteByTemplateAsync(UserQueries.Create, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserQueries.Delete, new { Id = id });

    public async Task<List<User>> GetAsync(int range) =>
        await QueryListAsync(UserQueries.GetRangeWithoutCredentials, new { Count = range });

    public async Task<User?> GetByEmailAsync(string email) =>
        await QueryFirstAsync(UserQueries.GetByEmail, new { Email = email });

    public async Task<User?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(UserQueries.GetByIdWithoutCredentials, new { Id = id });

    public async Task UpdateAsync(User entity) =>
        await ExecuteByTemplateAsync(UserQueries.Update, entity);

    public async Task UpdateCredentialsAsync(User entity) =>
        await ExecuteByTemplateAsync(UserQueries.UpdateCredentials, entity);
}
