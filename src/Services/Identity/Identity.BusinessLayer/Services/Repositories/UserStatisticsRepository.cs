using Dapper;
using Identity.BusinessLayer.Contracts;
using Identity.DomainLayer.Entities;
using System.Data;
using System.Transactions;

namespace Identity.BusinessLayer.Services.Repositories
{
    public class UserStatisticsRepository : IUserStatisticsRepository
    {
        private static readonly string Get =
            "SELECT user_statistics.id, " +
            "total_words as \"TotalWords\", " +
            "total_hours as \"TotalHours\", " +
            "visit_streak as \"VisitStreak\", " +
            "avg_words as \"AvgWords\", " +
            "last_update_at as \"LastUpdateAt\", " +
            "users.id, " +
            "users.email as \"Email\", " +
            "users.phone as \"Phone\" " +
            "FROM user_statistics " +
            "INNER JOIN users ON user_statistics.user_id = users.id ";
        private static readonly string GetCount =
            "SELECT COUNT(*) FROM user_statistics ";
        private static readonly string GetCountByUserId = GetCount +
            "WHERE user_id = @Id";
        private static readonly string GetRange = Get +
            "LIMIT @Count";
        private static readonly string GetById = Get +
            "WHERE user_statistics.id = @Id";
        private static readonly string GetByUserId = Get +
            "WHERE user_statistics.user_id = @Id";
        private static readonly string Create =
            "INSERT INTO user_statistics " +
            "(id, total_words, total_hours, visit_streak, avg_words, user_id, last_update_at) " +
            "VALUES " +
            "(@Id, @TotalWords, @TotalHours, @VisitStreak, @AvgWords, @UserId, @LastUpdateAt)";
        private static readonly string Update =
            "UPDATE user_statistics " +
            "SET " +
            "total_words = @TotalWords, total_hours = @TotalHours, " +
            "visit_streak = @VisitStreak, avg_words = @AvgWords, user_id = @UserId " +
            "WHERE id = @Id";
        private static readonly string Delete =
            "DELETE FROM user_statistics " +
            "WHERE id = @Id";
        private readonly IDbConnection _connection;
        public UserStatisticsRepository(IDbConnection connection) =>
            _connection = connection;
        public async Task<UserStatistics> AddAsync(UserStatistics entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Create, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Delete, new { Id = id });

            return true;
        }

        public async Task<List<UserStatistics>> GetAsync(int count)
        {
            IEnumerable<UserStatistics> statistics;

            statistics = await _connection.QueryAsync<UserStatistics, User, UserStatistics>(GetRange,
                (statistics, user) =>
                {
                    statistics.User = user;
                    statistics.UserId = user.Id;
                    return statistics;
                }, new { Count = count }, splitOn: "id");

            return statistics.ToList() is null ? new List<UserStatistics>() : statistics.ToList();
        }

        public async Task<UserStatistics?> GetByIdAsync(Guid id)
        {
            IEnumerable<UserStatistics> statistics;

            statistics = await _connection.QueryAsync<UserStatistics, User, UserStatistics>(GetById,
                (statistics, user) =>
                {
                    statistics.User = user;
                    statistics.UserId = user.Id;
                    return statistics;
                }, new { Id = id }, splitOn: "id");

            return statistics.FirstOrDefault() is null ? null : statistics.First();
        }

        public async Task<UserStatistics?> GetByUserIdAsync(Guid id)
        {
            IEnumerable<UserStatistics> statistics;

            statistics = await _connection.QueryAsync<UserStatistics, User, UserStatistics>(GetByUserId,
                (statistics, user) =>
                {
                    statistics.User = user;
                    statistics.UserId = user.Id;
                    return statistics;
                }, new { Id = id }, splitOn: "id");

            return statistics.FirstOrDefault() is null ? null : statistics.First();
        }

        public async Task<int> GetCountOfDaysByUserIdAsync(Guid id)
        {
            var result = await _connection.QueryAsync<int>(GetCountByUserId, new { Id = id });

            return result.First();
        }

        public async Task<UserStatistics> UpdateAsync(UserStatistics entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }
    }
}
