namespace DataAccess.Dapper.Postgres.RawQueries;
public static class WordTypeQueries
{
    public readonly static string Get =
        "SELECT id as \"Id\", type_name as \"TypeName\" " +
        "FROM word_types ";
    public readonly static string GetRange = Get +
        "LIMIT @Count";
    public readonly static string GetById = Get +
        "WHERE id = @Id";
    public readonly static string Create =
        "INSERT INTO word_types (id, type_name) " +
        "VALUES (@Id, @TypeName)";
    public readonly static string Update =
        "UPDATE word_types " +
        "SET type_name = @TypeName " +
        "WHERE id = @ id";
    public readonly static string Delete =
        "DELETE FROM word_types WHERE id = @Id";
}
