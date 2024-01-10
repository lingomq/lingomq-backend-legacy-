namespace Identity.DataAccess.Providers.Dapper.RawQueries;
public static class UserQueries
{
    public static readonly string SelectAll =
        @"SELECT users.id,
          users.nickname as ""Nickname"",
          users.image_uri as ""ImageUri"", 
          roles.id, 
          roles.name as ""Name""
          FROM users 
          LEFT JOIN roles ON users.role_id = roles.id";
    public static readonly string SelectById =
        SelectAll + " WHERE id = @Id";
    public static readonly string SelectRange =
        SelectAll + " ORDER BY nickname DESC OFFSET (@Skip) ROWS FETCH NEXT (@Take) ROWS ONLY";
    public static readonly string Insert =
        "INSERT INTO users, " +
        "(id, nickname, image_uri, role_id) " +
        "VALUES " +
        "(@Id, @Nickname, @ImageUri, @RoleId)";
    public static readonly string Update =
        "UPDATE users " +
        "SET nickname = @Nickname, " +
        "image_uri = @ImageUri, " +
        "role_id = @RoleId " +
        "WHERE id = @Id";
    public static readonly string Delete =
        "DELETE FROM users " +
        "WHERE id = @Id";
}
