using System.Data;
using Notifications.DataAccess.Dapper.Contracts;
using Notifications.DataAccess.Dapper.Postgres.RawQueries;
using Notifications.DataAccess.Dapper.Utils;
using Notifications.Domain.Entities;

namespace Notifications.DataAccess.Dapper.Postgres.Realizations;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IDbConnection connection) : base(connection)
    {
    }

    public async Task<User?> GetAsync(Guid id) =>
        await QueryFirstAsync(UserQueries.GetById, new { Id = id });

    public async Task<List<User>> GetAsync(int count) =>
        await QueryListAsync(UserQueries.GetRange, new { Count = count });

    public async Task CreateAsync(User entity) =>
        await ExecuteByTemplateAsync(UserQueries.Create, entity);

    public async Task UpdateAsync(User entity) =>
        await ExecuteByTemplateAsync(UserQueries.Update, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserQueries.Delete, new { Id = id });
}
