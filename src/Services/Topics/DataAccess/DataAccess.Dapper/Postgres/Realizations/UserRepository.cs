using System.Data;
using Topics.DataAccess.Dapper.Contracts;
using Topics.DataAccess.Dapper.Utils;
using Topics.Domain.Entities;
using Topics.DataAccess.Dapper.Postgres.RawQueries;

namespace Topics.DataAccess.Dapper.Postgres.Realizations;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IDbConnection connection) : base(connection)
    {
    }

    public async Task AddAsync(User entity) =>
        await ExecuteByTemplateAsync(UserQueries.Create, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserQueries.Delete, new { Id = id });

    public async Task<List<User>> GetAsync(int range = int.MaxValue) =>
        await QueryListAsync(UserQueries.GetRange, new { Count = range });

    public async Task<User?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(UserQueries.GetById, new { Id = id });

    public async Task UpdateAsync(User entity) =>
        await ExecuteByTemplateAsync(UserQueries.Update, entity);
}
