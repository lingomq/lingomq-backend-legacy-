namespace Identity.DataAccess.Providers.Dapper.RawQueries;
public static class UserSensitiveDataQueries
{
    private static readonly string Select =
        @"SELECT user_sensitive_datas.id,
        user_sensitive_datas.password_hash as ""PasswordHash"", 
        user_sensitive_datas.password_salt as ""PasswordSalt"", 
        users.id, 
        users.nickname as ""Nickname"", 
        users.image_uri as ""ImageUri"", 
        roles.id, 
        roles.name as ""Name"" 
        FROM user_sensitive_datas 
        LEFT JOIN users ON users.id = user_sensitive_datas.user_id 
        LEFT JOIN roles ON users.role_id = roles.id";
    public static readonly string SelectByUserId =
        Select + " WHERE user_id = @UserId";
    public static readonly string Insert = 
        "INSERT INTO user_sensitive_datas " +
        "(id, password_hash, password_salt, user_id) " +
        "VALUES " +
        "(@Id, @PasswordHash, @PasswordSalt, @UserId)";
    public static readonly string Update =
        "UPDATE user_sensitive_datas " +
        "SET password_hash = @PasswordHash, " +
        "password_salt = @PasswordSalt, " +
        "user_id = @UserId " +
        "WHERE id = @Id";
    public static readonly string UpdateByUserId =
        "UPDATE user_sensitive_datas " +
        "SET password_hash = @PasswordHash, " +
        "password_salt = @PasswordSalt, " +
        "WHERE user_id = @UserId";
    public static readonly string Delete =
        "DELETE FROM user_sensitive_datas " +
        "WHERE id = @Id";
    public static readonly string DeleteByUserId =
        "DELETE FROM user_sensitive_datas " +
        "WHERE user_id = @UserId";
}
