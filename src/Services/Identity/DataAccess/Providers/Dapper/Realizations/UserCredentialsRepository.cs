using Dapper;
using Identity.DataAccess.Contracts;
using Identity.DataAccess.Providers.Dapper.RawQueries;
using Identity.DataAccess.Utils;
using Identity.Domain.Entities;
using System.Data;

namespace Identity.DataAccess.Providers.Dapper.Realizations;
public class UserCredentialsRepository : GenericRepository<UserCredentials>, IUserCredentialsRepository
{
    private readonly IDbConnection _connection;
    public UserCredentialsRepository(IDbConnection connection) : base(connection)
    {
        _connection = connection;
    }

    public async Task AddAsync(UserCredentials entity) =>
        await ExecuteByTemplateAsync(UserCredentialsQueries.Insert, entity);
    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserCredentialsQueries.Delete, new { Id = id });
    public async Task<List<UserCredentials>> GetAsync(int skip, int take) =>
        await QueryListAsync(UserCredentialsQueries.SelectAll, new { Skip = skip, Take = take });

    public async Task<UserCredentials?> GetByUserIdAsync(Guid id) =>
        await QueryFirstAsync(UserCredentialsQueries.SelectByUserId, new { Id = id }); 

    public async Task UpdateAsync(UserCredentials entity) =>
        await ExecuteByTemplateAsync(UserCredentialsQueries.Update, entity);
    protected override async Task<List<UserCredentials>> QueryListAsync<E>(string sql, E entity)
    {
        IEnumerable<UserCredentials> entities;
        
        entities = await _connection.QueryAsync<UserCredentials, User, Role, UserCredentials>(sql,
            (credentials, user, role) =>
            {
                credentials.User = user;
                credentials.User.Role = role;
                credentials.UserId = user.Id;
                credentials.User.RoleId = role.Id;

                return credentials;
            },
            entity, splitOn: "id");

        return entities.Any() ? entities.ToList() : new List<UserCredentials>();
    }
}
