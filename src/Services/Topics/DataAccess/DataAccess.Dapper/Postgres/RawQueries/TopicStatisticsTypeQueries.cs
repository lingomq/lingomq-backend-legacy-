namespace Topics.DataAccess.Dapper.Postgres.RawQueries;
public static class TopicStatisticsTypeQueries
{
    public readonly static string Get =
            "SELECT id as \"Id\", " +
            "name as \"TypeName\" " +
            "FROM topic_statistics_types ";
    public readonly static string GetRange = Get +
        "LIMIT @Count";
    public readonly static string GetById = Get +
        "WHERE id = @Id";
    public readonly static string Create =
        "INSERT INTO topic_statistics_types " +
        "(id, name) " +
        "VALUES (@Id, @TypeName)";
    public readonly static string Update =
        "UPDATE topic_statistics_types SET" +
        "name = @TypeName " +
        "WHERE id = @Id";
    public readonly static string Delete =
        "DELETE FROM topic_statistics_types " +
        "WHERE id = @Id";
}
