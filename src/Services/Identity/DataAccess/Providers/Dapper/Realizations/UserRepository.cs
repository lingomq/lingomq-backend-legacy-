using Dapper;
using Identity.DataAccess.Contracts;
using Identity.DataAccess.Providers.Dapper.RawQueries;
using Identity.DataAccess.Utils;
using Identity.Domain.Entities;
using System.Data;

namespace Identity.DataAccess.Providers.Dapper.Realizations;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly IDbConnection _connection;
    public UserRepository(IDbConnection connection) : base(connection)
    {
        _connection = connection;
    }

    public async Task AddAsync(User entity) =>
        await ExecuteByTemplateAsync(UserQueries.Insert, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserQueries.Delete, new { Id = id });

    public async Task<List<User>> GetAsync(int skip, int take) =>
        await QueryListAsync(UserQueries.SelectRange, new { Skip = skip, Take = take });

    public async Task<User?> GetAsync(Guid id) =>
        await QueryFirstAsync(UserQueries.SelectById, new { Id = id });

    public async Task UpdateAsync(User entity) =>
        await ExecuteByTemplateAsync(UserQueries.Update, entity);

    protected override async Task<List<User>> QueryListAsync<E>(string sql, E entity)
    {
        IEnumerable<User> users;
        users = await _connection.QueryAsync<User, Role, User>(sql,
            (user, role) =>
            {
                user.Role = role;
                user.RoleId = role.Id;

                return user;
            }, entity, splitOn: "id");

        return users.Any() ? users.ToList() : new List<User>();
    }
}
