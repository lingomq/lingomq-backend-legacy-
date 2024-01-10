namespace Identity.DataAccess.Providers.Dapper.RawQueries;
public static class UserInfoQueries
{
    public static readonly string SelectAll =
        @"SELECT user_infos.id, user_infos.creation_time as ""CreationTime"", 
        user_infos.description as ""Description"", 
        user_infos.locale as ""Locale"", 
        users.id,  
        users.nickname as ""Nickname"", 
        users.image_uri as ""ImageUri"", 
        roles.id, 
        roles.name as ""Name"" 
        FROM user_infos 
        LEFT JOIN users ON users.id = user_infos.user_id 
        LEFT JOIN roles ON roles.id = users.role_id";
    public static readonly string SelectByUserId =
        SelectAll + " WHERE user_id = @UserId";
    public static readonly string Insert =
        "INSERT INTO user_infos " +
        "(id, creation_time, description, locale, user_id) " +
        "VALUES " +
        "(@Id, @CreationTime, @Description, @Locale, @UserId);";
    public static readonly string Update =
        "UPDATE user_infos " +
        "SET creation_time = @CreationTime, " +
        "description = @Description, " +
        "locale = @Locale, " +
        "user_id = @UserId " +
        "WHERE id = @Id";
    public static readonly string Delete =
        "DELETE FROM user_infos " +
        "WHERE id = @Id";
}
