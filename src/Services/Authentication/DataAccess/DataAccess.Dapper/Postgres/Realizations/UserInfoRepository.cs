using Authentication.DataAccess.Dapper.Contracts;
using Authentication.DataAccess.Dapper.Postgres.RawQueries;
using Authentication.DataAccess.Dapper.Utils;
using Authentication.Domain.Entities;
using Dapper;
using System.Data;

namespace Authentication.DataAccess.Dapper.Postgres.Realizations;
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

    public async Task UpdateAsync(UserInfo entity) =>
        await ExecuteByTemplateAsync(UserInfoQueries.Update, entity);

    protected override async Task<List<UserInfo>> QueryListAsync<E>(string sql, E entity)
    {
        IEnumerable<UserInfo> userInfos = await _connection
                .QueryAsync<UserInfo, UserRole, User, UserInfo>(sql,
                (userInfo, role, user) =>
                {
                    userInfo.Role = role;
                    userInfo.RoleId = role.Id;
                    userInfo.User = user;
                    userInfo.UserId = user.Id;
                    return userInfo;
                },
                entity, splitOn: "id");

        return userInfos.Any() ? userInfos.ToList() : new List<UserInfo>();
    }

    protected override async Task<UserInfo?> QueryFirstAsync<E>(string sql, E entity)
    {
        List<UserInfo> userInfos = await QueryListAsync(sql, entity);
        return userInfos.Any() ? userInfos.First() : null;
    }
}
