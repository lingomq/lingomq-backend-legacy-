using Identity.DataAccess.Contracts;
using Identity.DataAccess.Providers.Dapper.RawQueries;
using Identity.DataAccess.Utils;
using Identity.Domain.Entities;
using System.Data;

namespace Identity.DataAccess.Providers.Dapper.Realizations;
public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(IDbConnection connection) : base(connection)
    {
    }

    public async Task AddAsync(Role entity) =>
        await ExecuteByTemplateAsync(RoleQueries.Insert, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(RoleQueries.Delete, new { Id = id });

    public async Task<List<Role>> GetAsync(int skip, int take) =>
        await QueryListAsync(RoleQueries.SelectRange, new { Skip = skip, Take = take });

    public async Task UpdateAsync(Role entity) =>
        await ExecuteByTemplateAsync(RoleQueries.Update, entity);
}
