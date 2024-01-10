using Dapper;
using Identity.DataAccess.Contracts;
using Identity.DataAccess.Providers.Dapper.RawQueries;
using Identity.DataAccess.Utils;
using Identity.Domain.Entities;
using System.Data;

namespace Identity.DataAccess.Providers.Dapper.Realizations;
public class UserInfoRepository : GenericRepository<UserInfo>, IUserInfoRepository
{
    private readonly IDbConnection _connection;
    public UserInfoRepository(IDbConnection connection) : base(connection)
    {
        _connection = connection;
    }

    public async Task AddAsync(UserInfo entity) =>
        await ExecuteByTemplateAsync(UserInfoQueries.Insert, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserInfoQueries.Delete, new { Id = id });

    public async Task<List<UserInfo>> GetAsync(int skip, int take) =>
        await QueryListAsync(UserInfoQueries.SelectAll, new { skip = skip, Take = take });

    public async Task<UserInfo?> GetByUserIdAsync(Guid id) =>
        await QueryFirstAsync(UserInfoQueries.SelectByUserId, new { UserId = id });

    public async Task UpdateAsync(UserInfo entity) =>
        await ExecuteByTemplateAsync(UserInfoQueries.Update, entity);

    protected override async Task<List<UserInfo>> QueryListAsync<E>(string sql, E entity)
    {
        IEnumerable<UserInfo> infos;
        infos = await _connection.QueryAsync<UserInfo, User, Role, UserInfo>(sql, 
            (info, user, role) =>
            {
                info.User = user;
                info.User.Role = role;
                info.UserId = user.Id;
                info.User.RoleId = role.Id;

                return info;
            }, entity, splitOn: "id");

        return infos.Any() ? infos.ToList() : new List<UserInfo>();
    }
}
