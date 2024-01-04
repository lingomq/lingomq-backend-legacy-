namespace DataAccess.Dapper.Postgres.RawQueries;
public static class UserStatisticsQueries
{
    public static readonly string Get =
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
    public static readonly string GetCount =
        "SELECT COUNT(*) FROM user_statistics ";
    public static readonly string GetCountByUserId = GetCount +
        "WHERE user_id = @Id";
    public static readonly string GetRange = Get +
        "LIMIT @Count";
    public static readonly string GetById = Get +
        "WHERE user_statistics.id = @Id";
    public static readonly string GetByUserId = Get +
        "WHERE user_statistics.user_id = @Id";
    public static readonly string Create =
        "INSERT INTO user_statistics " +
        "(id, total_words, total_hours, visit_streak, avg_words, user_id, last_update_at) " +
        "VALUES " +
        "(@Id, @TotalWords, @TotalHours, @VisitStreak, @AvgWords, @UserId, @LastUpdateAt)";
    public static readonly string Update =
        "UPDATE user_statistics " +
        "SET " +
        "total_words = @TotalWords, total_hours = @TotalHours, " +
        "visit_streak = @VisitStreak, avg_words = @AvgWords, user_id = @UserId " +
        "WHERE id = @Id";
    public static readonly string Delete =
        "DELETE FROM user_statistics " +
        "WHERE id = @Id";
}
