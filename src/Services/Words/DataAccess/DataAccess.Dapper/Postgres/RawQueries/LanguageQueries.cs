namespace DataAccess.Dapper.Postgres.RawQueries;
public static class LanguageQueries
{
    public readonly static string Get =
        "SELECT id as \"Id\", name as \"Name\" " +
        "FROM languages ";
    public readonly static string GetRange = Get +
        "LIMIT @Count";
    public readonly static string GetById = Get +
        "WHERE id = @Id";
    public readonly static string GetByLanguageName = Get +
        "WHERE name = @Name";
    public readonly static string Create =
        "INSERT INTO languages (id, name) " +
        "VALUES (@Id, @Name)";
    public readonly static string Update =
        "UPDATE languages " +
        "SET name = @Name " +
        "WHERE id = @Id";
    public readonly static string Delete =
        "DELETE FROM languages " +
        "WHERE id = @Id";
}
