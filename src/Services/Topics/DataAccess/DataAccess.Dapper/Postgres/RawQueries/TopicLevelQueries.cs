namespace Topics.DataAccess.Dapper.Postgres.RawQueries;
public static class TopicLevelQueries
{
    public readonly static string Get =
            "SELECT id as \"Id\", " +
            "name as \"LevelName\" " +
            "FROM topic_levels ";
    public readonly static string GetById = Get +
        "WHERE id = @Id";
    public readonly static string GetRange = Get +
        "LIMIT @Count;";
    public readonly static string Create =
        "INSERT INTO topic_levels " +
        "(id, name) " +
        "VALUES (@Id, @LevelName)";
    public readonly static string Update =
        "UPDATE topic_levels SET " +
        "name = @LevelName " +
        "WHERE id = @Id";
    public readonly static string Delete =
        "DELETE FROM topic_levels " +
        "WHERE id = @Id";
}
