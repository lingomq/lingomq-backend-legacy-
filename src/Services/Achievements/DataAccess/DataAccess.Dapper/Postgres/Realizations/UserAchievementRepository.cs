using Achievements.DataAccess.Dapper.Contracts;
using Achievements.DataAccess.Dapper.Postgres.RawQueries;
using Achievements.DataAccess.Dapper.Utils;
using Achievements.Domain.Entities;
using Dapper;
using System.Data;

namespace Achievements.DataAccess.Dapper.Postgres.Realizations;
public class UserAchievementRepository : GenericRepository<UserAchievement>, IUserAchievementRepository
{
    private readonly IDbConnection _connection;
    public UserAchievementRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;

    public async Task CreateAsync(UserAchievement userAchievement) =>
        await ExecuteByTemplateAsync(UserAchievementQueries.Create, userAchievement);
    public async Task<List<UserAchievement>> GetAsync(int range) =>
        await QueryListAsync(UserAchievementQueries.GetRange, new { Count  = range });

    public async Task<UserAchievement?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(UserAchievementQueries.GetById, new { Id = id });
    
    public async Task<List<UserAchievement>> GetByUserIdAsync(Guid id) =>
        await QueryListAsync(UserAchievementQueries.GetByUserId, new { Id = id });
    
    public async Task<int> GetCountAchievementsByUserIdAsync(Guid id) =>
        await _connection.QueryFirstAsync(UserAchievementQueries.GetCountAchievementsByUserId, new { Id = id });

    protected override async Task<List<UserAchievement>> QueryListAsync<T>(string sql, T entity) where T : class
    {
        IEnumerable<UserAchievement>? usersAchievements;
        usersAchievements = await _connection.QueryAsync<UserAchievement, User, Achievement, UserAchievement>
            (sql, (userAchievement, user, achievement) =>
            {
                userAchievement.User = user;
                userAchievement.UserId = user.Id;

                userAchievement.Achievement = achievement;
                userAchievement.AchievementId = achievement.Id;

                return userAchievement;
            }, entity, splitOn: "id");

        return usersAchievements.Count() == 0 ? new List<UserAchievement>() : usersAchievements.ToList();
    }
}
