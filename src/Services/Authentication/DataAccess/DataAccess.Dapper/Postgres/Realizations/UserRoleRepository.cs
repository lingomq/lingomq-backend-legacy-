using Authentication.DataAccess.Dapper.Contracts;
using Authentication.DataAccess.Dapper.Postgres.RawQueries;
using Authentication.DataAccess.Dapper.Utils;
using Authentication.Domain.Entities;
using System.Data;

namespace Authentication.DataAccess.Dapper.Postgres.Realizations;
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
