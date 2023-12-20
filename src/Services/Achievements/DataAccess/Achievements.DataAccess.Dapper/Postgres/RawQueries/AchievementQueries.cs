namespace Achievements.DataAccess.Dapper.Postgres.RawQueries;
public static class AchievementQueries
{
    public readonly static string Get =
        "SELECT id as \"Id\"," +
        "name as \"Name\"," +
        "content as \"Content\"," +
        "image_uri as \"ImageUri\" " +
        "FROM achievements ";
    public readonly static string GetRange = Get +
                                              "LIMIT @Count";
    public readonly static string GetById = Get +
                                             "WHERE id = @Id";
    public readonly static string Create =
        "INSERT INTO achievements " +
        "(id, name, content, image_uri) " +
        "VALUES (@Id, @Name, @Content, @ImageUri)";
    public readonly static string Update =
        "UPDATE achievements SET" +
        "name = @Name, " +
        "content = @Content, " +
        "image_uri = @ImageUri " +
        "WHERE id = @Id";
    public readonly static string Delete =
        "DELETE FROM achievements " +
        "WHERE id = @Id";
}
