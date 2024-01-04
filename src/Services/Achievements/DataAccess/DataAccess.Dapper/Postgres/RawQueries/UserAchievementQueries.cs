namespace Achievements.DataAccess.Dapper.Postgres.RawQueries;
public static class UserAchievementQueries
{
    public readonly static string Get =
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
            "JOIN achievements ON achievements.id = user_achievements.achievement_id ";
    public readonly static string GetRange = Get +
        "LIMIT @Count";
    public readonly static string GetById = Get +
        "WHERE id = @Id";
    public readonly static string GetByUserId = Get +
        "WHERE users.id = @Id";
    public readonly static string GetCountAchievementsByUserId =
        "SELECT COUNT(*) FROM user_achievements WHERE user_id = @Id";
    public readonly static string Create =
        "INSERT INTO user_achievements " +
        "(id, date_of_receipt, user_id, achievement_id) " +
        "VALUES " +
        "(@Id, @DateOfReceipt, @UserId, @AchievementId)";
}
