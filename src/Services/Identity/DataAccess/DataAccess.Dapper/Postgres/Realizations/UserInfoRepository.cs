using Dapper;
using Identity.DataAccess.Dapper.Contracts;
using Identity.DataAccess.Dapper.Postgres.RawQueries;
using Identity.DataAccess.Dapper.Utils;
using Identity.Domain.Entities;
using System.Data;

namespace Identity.DataAccess.Dapper.Postgres.Realizations;
public class UserInfoRepository : GenericRepository<UserInfo>, IUserInfoRepository
{
    private readonly IDbConnection _connection;
    public UserInfoRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;
    public async Task AddAsync(UserInfo entity) =>
        await ExecuteByTemplateAsync(UserInfoQueries.Create, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserInfoQueries.Delete, new { Id = id });

    public async Task<List<UserInfo>> GetAsync(int range) =>
        await QueryListAsync(UserInfoQueries.GetRange, new { Count = range });

    public async Task<UserInfo?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(UserInfoQueries.GetById, new { Id = id });

    public async Task<UserInfo?> GetByNicknameAsync(string nickname) =>
        await QueryFirstAsync(UserInfoQueries.GetByNickname, new { Nickname = nickname });

    public async Task<UserInfo?> GetByUserIdAsync(Guid id) =>
        await QueryFirstAsync(UserInfoQueries.GetByUserId, new { UserId = id });

    public async Task UpdateAsync(UserInfo entity) =>
        await ExecuteByTemplateAsync(UserInfoQueries.Update, entity);

    protected override async Task<List<UserInfo>> QueryListAsync<E>(string sql, E entity)
    {
        IEnumerable<UserInfo> infos;

        infos = await _connection.QueryAsync<UserInfo, UserRole, User, UserInfo>(sql,
            (info, role, user) =>
            {
                info.Role = role;
                info.RoleId = role.Id;
                info.User = user;
                info.UserId = user.Id;
                return info;
            }, entity, splitOn: "id");

        return infos.Any() ? infos.ToList() : new List<UserInfo>();
    }

    protected override async Task<UserInfo?> QueryFirstAsync<E>(string sql, E entity)
    {
        List<UserInfo> infos = await QueryListAsync(sql, entity);
        return infos.Any() ? infos.FirstOrDefault() : null;
    }
}
