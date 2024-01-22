using Dapper;
using Identity.DataAccess.Dapper.Contracts;
using Identity.DataAccess.Dapper.Postgres.RawQueries;
using Identity.DataAccess.Dapper.Utils;
using Identity.Domain.Entities;
using System.Data;

namespace Identity.DataAccess.Dapper.Postgres.Realizations;
public class UserStatisticsRepository : GenericRepository<UserStatistics>, IUserStatisticsRepository
{
    private readonly IDbConnection _connection;
    public UserStatisticsRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;
    public async Task AddAsync(UserStatistics entity) =>
        await ExecuteByTemplateAsync(UserStatisticsQueries.Create, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserStatisticsQueries.Delete, new { Id = id });

    public async Task<List<UserStatistics>> GetAsync(int range) =>
        await QueryListAsync(UserStatisticsQueries.GetRange, new { Count = range });

    public async Task<UserStatistics?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(UserStatisticsQueries.GetById, new { Id = id });

    public async Task<UserStatistics?> GetByUserIdAsync(Guid id) =>
        await QueryFirstAsync(UserStatisticsQueries.GetByUserId, new { Id = id });

    public async Task<int> GetCountOfDaysByUserIdAsync(Guid id) =>
        await _connection.QueryFirstAsync<int>(UserStatisticsQueries.GetCountByUserId, new { Id = id });

    public async Task UpdateAsync(UserStatistics entity) =>
        await ExecuteByTemplateAsync(UserStatisticsQueries.Update, entity);

    protected override async Task<List<UserStatistics>> QueryListAsync<E>(string sql, E entity)
    {
        IEnumerable<UserStatistics> statistics;

        statistics = await _connection.QueryAsync<UserStatistics, User, UserStatistics>(sql,
            (statistics, user) =>
            {
                statistics.User = user;
                statistics.UserId = user.Id;
                return statistics;
            }, entity, splitOn: "id");

        return statistics.Any() ? statistics.ToList() : new List<UserStatistics>();
    }

    protected override async Task<UserStatistics?> QueryFirstAsync<E>(string sql, E entity)
    {
        List<UserStatistics> statistics = await QueryListAsync(sql, entity);
        return statistics.Any() ? statistics.First() : null;
    }
}
