namespace Identity.DataAccess.Dapper.Postgres.RawQueries;
public static class UserRoleQueries
{
    public static readonly string Get =
            "SELECT id as \"Id\", name as \"Name\" " +
            "FROM user_roles";
    public static readonly string GetById = Get +
        " WHERE id = @Id";
    public static readonly string GetByName = Get +
        " WHERE name = @Name";
    public static readonly string GetRange = Get +
        " LIMIT @Count";
    public static readonly string Create =
        "INSERT INTO user_roles " +
        "(id, name) " +
        "VALUES " +
        "(@Id, @Name);";
    public static readonly string Delete =
        "DELETE FROM user_roles " +
        "WHERE id = @Id";
    public static readonly string Update =
        "UPDATE user_roles " +
        "SET name = @Name " +
        "WHERE id = @Id";
}
