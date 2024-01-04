namespace Topics.DataAccess.Dapper.Postgres.RawQueries;
public static class TopicQueries
{
    public static readonly string Get =
            "SELECT topics.id, " +
            "title as \"Title\", " +
            "content as \"Content\", " +
            "icon as \"Icon\"," +
            "creational_date as \"CreationalDate\", " +
            "languages.id, " +
            "languages.name as \"Name\", " +
            "topic_levels.id, " +
            "topic_levels.name as \"LevelName\" " +
            "FROM topics " +
            "JOIN languages ON languages.id = topics.language_id " +
            "JOIN topic_levels ON topic_levels.id = topics.topic_level_id ";
    public static readonly string GetRange = Get;
    public static readonly string PaginationAndOrderByDate = "ORDER BY creational_date DESC OFFSET (@Skip) ROWS " +
        "FETCH NEXT (@Take) ROWS ONLY ";
    public static readonly string GetById = Get + "WHERE topics.id = @Id";
    public static readonly string GetByDateRange = Get +
        "WHERE creational_date > @StartDate " +
        "AND creational_date < @EndDate ";
    public static readonly string GetByLanguageId = "AND languages.id = @LanguageId ";
    public static readonly string GetByTopicLevelId = "AND topic_levels.id = @LevelId ";
    public static readonly string GetBySearch = "AND title LIKE @Search AND content LIKE @Search ";
    public static readonly string Create =
        "INSERT INTO topics " +
        "(id, title, content, icon, creational_date, language_id, topic_level_id) " +
        "VALUES " +
        "(@Id, @Title, @Content, @Icon, @CreationalDate, @LanguageId, @TopicLevelId)";
    public static readonly string Update =
        "UPDATE topics SET" +
        "title = @Title, " +
        "content = @Content, " +
        "icon = @Icon, " +
        "creational_date = @CreationalDate, " +
        "language_id = @LanguageId, " +
        "topic_level_id = @TopicLevelId " +
        "WHERE id = @Id";
    public static readonly string Delete =
        "DELETE FROM topics " +
        "WHERE id = @Id";
    public static readonly string Limit = "LIMIT @Count ";
}
