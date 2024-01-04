namespace Authentication.DataAccess.Dapper.Postgres.RawQueries;
public static class UserInfoQueries
{
    public readonly static string Get =
            "SELECT user_infos.id, " +
            "user_infos.nickname AS \"Nickname\", " +
            "user_infos.image_uri AS \"ImageUri\", " +
            "user_infos.additional AS \"Additional\", " +
            "user_infos.creational_date AS \"CreationalDate\", " +
            "user_infos.is_removed AS \"IsRemoved\", " +
            "user_roles.id, " +
            "user_roles.name AS \"Name\", " +
            "users.id, " +
            "users.email AS \"Email\", " +
            "users.phone AS \"Phone\", " +
            "users.password_hash AS \"PasswordHash\", " +
            "users.password_salt AS \"PasswordSalt\" " +
            "FROM user_infos " +
            "LEFT JOIN user_roles ON user_infos.role_id = user_roles.id " +
            "LEFT JOIN users ON user_infos.user_id = users.id ";
    public readonly static string GetRange = Get +
        " LIMIT @Count";
    public readonly static string GetByNickname = Get +
        " WHERE user_infos.nickname = @Nickname";
    public readonly static string GetById = Get +
        " WHERE user_infos.id = @Id";
    public readonly static string Update =
            "UPDATE user_infos " +
            "SET nickname = @Nickname," +
            "image_uri = @Phone," +
            "additional = @PasswordHash," +
            "role_id = @PasswordSalt " +
            "user_id = @UserId " +
            "is_removed = @IsRemoved " +
            "WHERE id = @Id";
    public readonly static string Delete =
            "DELETE FROM user_infos " +
            "WHERE id = @Id";
    public readonly static string Create =
           "INSERT INTO user_infos (id, nickname, image_uri, additional, role_id, user_id, is_removed) " +
           "VALUES (@Id, @Nickname, @ImageUri, @Additional, @RoleId, @UserId, @IsRemoved);";
}
