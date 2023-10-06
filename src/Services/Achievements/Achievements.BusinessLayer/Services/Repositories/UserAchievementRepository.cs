using Achievements.BusinessLayer.Contracts;
using Achievements.DomainLayer.Entities;
using Dapper;
using System.Data;
using System.Transactions;

namespace Achievements.BusinessLayer.Services.Repositories
{
    public class UserAchievementRepository : IUserAchievementRepository
    {
        private readonly static string Get =
            "SELECT user_achievements.id, " +
            "date_of_receipt as \"DateOfReceipt\", " +
            "users.id, " +
            "users.email as \"Email\", " +
            "users.phone as \"Phone\", " +
            "achievements.id, " +
            "achievements.name as \"Name\", " +
            "achievements.content as \"Content\"," +
            "achievements.image_uri as \"ImageUri\" " +
            "FROM user_achievements " +
            "JOIN users ON users.id = user_achievements.user_id " +
            "JOIN achievements ON achievements.id = user_achievements.achivement_id ";
        private readonly static string GetRange = Get +
            "LIMIT @Count";
        private readonly static string GetById = Get +
            "WHERE id = @Id";
        private readonly static string GetByUserId = Get +
            "WHERE users.id = @Id";
        private readonly static string GetCountAchievementsByUserId =
            "SELECT COUNT(*) FROM user_achievements WHERE user_id = @Id";
        private readonly static string Create =
            "INSERT INTO user_achievements " +
            "(id, user_id, achievement_id, date_of_receipt) " +
            "VALUES (@Id, @UserId, @AchievementId, @DateOfReceipt)";
        private readonly static string Update =
            "UPDATE user_achievements SET " +
            "user_id = @UserId, " +
            "achievement_id = @AchievementId, " +
            "date_of_receipt = @DateOfReceipt " +
            "WHERE id = @Id";
        private readonly static string Delete =
            "DELETE FROM user_achievemets " +
            "WHERE id = @Id";

        private readonly IDbConnection _connection;
        public UserAchievementRepository(IDbConnection connection) =>
            _connection = connection;
        public async Task CreateAsync(UserAchievement entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Create, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Delete, new { Id = id });
            transactionScope.Complete();
            transactionScope.Dispose();
        }

        public async Task<List<UserAchievement>> GetAsync(int range)
        {
            return await GetByTemplate(GetRange, new { Count = range });
        }

        public async Task<UserAchievement?> GetByIdAsync(Guid id)
        {
            List<UserAchievement> userAchievements = await GetByTemplate(GetById, new { Id = id });
            return userAchievements.FirstOrDefault();
        }

        public async Task<List<UserAchievement>> GetByUserIdAsync(Guid id)
        {
            return await GetByTemplate(GetByUserId, new { Id = id });
        }

        public async Task<int> GetCountAchievementsByUserIdAsync(Guid id)
        {
            IEnumerable<int> result;

            result = await _connection.QueryAsync<int>(GetCountAchievementsByUserId, new { Id = id });
            return result.FirstOrDefault();
        }

        public async Task UpdateAsync(UserAchievement entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }

        private async Task<List<UserAchievement>> GetByTemplate<T>(string sql, T entity) where T : class
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
}
