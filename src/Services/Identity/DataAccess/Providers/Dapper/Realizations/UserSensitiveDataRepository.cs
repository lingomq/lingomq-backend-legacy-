using Dapper;
using Identity.DataAccess.Contracts;
using Identity.DataAccess.Providers.Dapper.RawQueries;
using Identity.DataAccess.Utils;
using Identity.Domain.Entities;
using System.Data;

namespace Identity.DataAccess.Providers.Dapper.Realizations;
public class UserSensitiveDataRepository : GenericRepository<UserSensitiveData>, IUserSensitiveDataRepository
{
    private readonly IDbConnection _connection;
    public UserSensitiveDataRepository(IDbConnection connection) : base(connection)
    {
        _connection = connection;
    }

    public async Task AddAsync(UserSensitiveData entity) => 
        await ExecuteByTemplateAsync(UserSensitiveDataQueries.Insert, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserSensitiveDataQueries.Delete, new { Id = id });

    public async Task<UserSensitiveData?> GetByUserIdAsync(Guid id) =>
        await QueryFirstAsync(UserSensitiveDataQueries.SelectByUserId, new { UserId = id });

    public async Task<UserSensitiveData?> GetByUserNicknameAsync(string nickname) =>
        await QueryFirstAsync(UserSensitiveDataQueries.SelectByNickname, new { Nickname = nickname });
    public async Task UpdateAsync(UserSensitiveData entity) =>
        await ExecuteByTemplateAsync(UserSensitiveDataQueries.Update, entity);

    protected override async Task<List<UserSensitiveData>> QueryListAsync<E>(string sql, E entity)
    {
        IEnumerable<UserSensitiveData> sensitiveData;
        sensitiveData = await _connection.QueryAsync<UserSensitiveData, User, Role, UserSensitiveData>(sql,
            (sensitiveData, user, role) =>
            {
                sensitiveData.User = user;
                sensitiveData.UserId = user.Id;
                user.Role = role;
                user.RoleId = role.Id;

                return sensitiveData;
            }, entity, splitOn: "id");

        return sensitiveData.Any() ? sensitiveData.ToList() : new List<UserSensitiveData>();
    }
}
