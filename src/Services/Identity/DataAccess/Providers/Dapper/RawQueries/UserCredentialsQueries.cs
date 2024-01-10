namespace Identity.DataAccess.Providers.Dapper.RawQueries;
public static class UserCredentialsQueries
{
    public static readonly string SelectAll =
        @"SELECT user_credentials.id, 
        user_credentials.email as ""Email"", 
        user_credentials.phone as ""Phone"", 
        users.id, 
        users.nickname as ""Nickname"", 
        users.image_uri as ""ImageUri"", 
        roles.id, 
        roles.name as ""Name"" 
        FROM user_credentials 
        LEFT JOIN users ON users.id = user_credentials.user_id 
        LEFT JOIN roles ON users.role_id = roles.id";
    public static readonly string SelectByUserId =
        SelectAll + " WHERE user_id = @UserId";
    public static readonly string Insert =
        "INSERT INTO user_credentials " +
        "(id, email, phone, user_id) " +
        "VALUES " +
        "(@Id, @Email, @Phone, @UserId)";
    public static readonly string Update =
        "UPDATE user_credentials " +
        "SET email = @Email, " +
        "phone = @Phone," +
        "user_id = @UserId " +
        "WHERE id = @Id";
    public static readonly string UpdateByUserId = 
        "UPDATE user_credentials " +
        "SET email = @Email, " +
        "phone = @Phone " +
        "WHERE user_id = @UserId";
    public static readonly string Delete =
        "DELETE FROM user_credentials WHERE id = @Id";
    public static readonly string DeleteByUserId =
        "DELETE FROM user_credentials WHERE user_id = @UserId";
}
