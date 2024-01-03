namespace Topics.DataAccess.Dapper.Postgres.RawQueries;
public static class TopicStatisticsQueries 
{
    public static readonly string Get =
        "SELECT topic_statistics.id, " +
        "statistics_date as \"StatisticsDate\", " +
        "topics.id, " +
        "topics.title as \"Title\", " +
        "topics.content as \"Content\", " +
        "topics.icon as \"Icon\"," +
        "topics.creational_date as \"CreationalDate\", " +
        "languages.id, " +
        "languages.name as \"Name\", " +
        "topic_levels.id, " +
        "topic_levels.name as \"LevelName\"," +
        "users.id, " +
        "users.email as \"Email\", " +
        "users.phone as \"Phone\", " +
        "topic_statistics_types.id," +
        "topic_statistics_types.name as \"TypeName\" " +
        "FROM topic_statistics " +
        "JOIN topics ON topics.id = topic_statistics.topic_id " +
        "JOIN languages ON languages.id = topics.language_id" +
        "JOIN topic_levels ON topic_levels.id = topics.topic_level_id " +
        "JOIN users ON users.id = topic_statistics.user_id " +
        "JOIN topic_statistics_types ON topic_statistics_types.id = topic_statistics.topic_statistics_type_id ";
    public static readonly string GetRange = Get +
        "LIMIT @Count";
    public static readonly string GetById = Get +
        "WHERE topic_statistics.id = @Id";
    public static readonly string GetByTopicId = Get +
        "WHERE topics.id = @Id";
    public static readonly string Create =
        "INSERT INTO topic_statistics " +
        "(id, topic_id, user_id, topic_statistics_type_id, statistics_date) " +
        "VALUES " +
        "(@Id, @TopicId, @UserId, @StatisticsTypeId, @StatisticsDate)";
    public static readonly string Update =
        "UPDATE topic_statistics SET " +
        "topic_id = @TopicId, " +
        "user_id = @UserId, " +
        "statistics_type_id = @StatisticsTypeId," +
        "statistics_date = @StatisticsDate " +
        "WHERE id = @Id";
    public static readonly string Delete =
        "DELETE FROM topic_statistics " +
        "WHERE id = @Id";
}
