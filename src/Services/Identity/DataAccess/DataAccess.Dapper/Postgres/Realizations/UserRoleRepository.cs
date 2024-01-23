using Identity.DataAccess.Dapper.Contracts;
using Identity.DataAccess.Dapper.Postgres.RawQueries;
using Identity.DataAccess.Dapper.Utils;
using Identity.Domain.Entities;
using System.Data;

namespace Identity.DataAccess.Dapper.Postgres.Realizations;
public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    private readonly IDbConnection _connection;
    public UserRoleRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;
    public async Task AddAsync(UserRole entity) =>
        await ExecuteByTemplateAsync(UserRoleQueries.Create, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserRoleQueries.Delete, new { Id = id });

    public async Task<List<UserRole>> GetAsync(int range) =>
        await QueryListAsync(UserRoleQueries.GetRange, new { Count = range });

    public async Task<UserRole?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(UserRoleQueries.GetById, new { Id = id });

    public async Task<UserRole?> GetByNameAsync(string name) =>
        await QueryFirstAsync(UserRoleQueries.GetByName, new { Name = name });

    public async Task UpdateAsync(UserRole entity) =>
        await ExecuteByTemplateAsync(UserRoleQueries.Update, entity);
}
