namespace Identity.DataAccess.Providers.Dapper.RawQueries;
public static class RoleQueries
{
    public static readonly string SelectAll =
        @"SELECT id as ""Id"", name as ""Name"" FROM roles";
    public static readonly string SelectByName =
        SelectAll + " WHERE name = @Name";
    public static readonly string SelectRange = 
        SelectAll + " ORDER BY name DESC OFFSET (@Skip) ROWS " +
        "FETCH NEXT (@Take) ROWS ONLY ";
    public static readonly string Insert =
        "INSERT INTO roles " +
        "(id, name) " +
        "VALUES " +
        "(@Id, @Name);";
    public static readonly string Update =
        "UPDATE roles " +
        "SET name = @Name " +
        "WHERE id = @Id";
    public static readonly string Delete =
        "DELETE FROM roles " +
        "SET name = @Name " +
        "WHERE id = @Id";
}
