using System.Data;
using Achievements.DataAccess.Dapper.Contracts;
using Achievements.DataAccess.Dapper.Postgres.RawQueries;
using Achievements.DataAccess.Dapper.Utils;
using Achievements.Domain.Entities;

namespace Achievements.DataAccess.Dapper.Postgres.Realizations;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IDbConnection connection) : base(connection)
    {
    }

    public async Task<User?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(UserQueries.GetById, new { Id = id });

    public async Task<List<User>> GetAsync(int range) =>
        await QueryListAsync(UserQueries.GetRange, new { Count = range });

    public async Task AddAsync(User entity) =>
        await ExecuteByTemplateAsync(UserQueries.Create, entity);

    public async Task UpdateAsync(User entity) =>
        await ExecuteByTemplateAsync(UserQueries.Update, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserQueries.Delete, new { Id = id });

    public async Task<User?> GetByEmailAsync(string email) =>
        await QueryFirstAsync(UserQueries.GetByEmail, new { Email = email });
}
