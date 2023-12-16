using Authentication.DataAccess.Dapper.Utils;
using Authentication.DataAccess.Dapper.Contracts;
using Authentication.Domain.Entities;
using System.Data;
using Authentication.DataAccess.Dapper.Postgres.RawQueries;

namespace Authentication.DataAccess.Dapper.Postgres.Realizations;
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
        await QueryListAsync(UserQueries.GetRange, new { Count = range });

    public async Task<User?> GetByEmailAsync(string email) =>
        await QueryFirstAsync(UserQueries.GetByEmail, new { Email = email });

    public async Task<User?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(UserQueries.GetById, new { Id = id });

    public async Task UpdateAsync(User entity) =>
        await ExecuteByTemplateAsync(UserQueries.Update, entity);

    public async Task<User?> UpdateCredentials(User user) 
    {
        await ExecuteByTemplateAsync(UserQueries.UpdateCredential, user);
        return user;
    }
}
