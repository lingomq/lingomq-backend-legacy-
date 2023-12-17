namespace DataAccess.Dapper.Postgres.RawQueries;
public static class UserWordQueries
{
    public readonly static string Get =
            "SELECT user_words.id, " +
            "word as \"Word\", " +
            "translated as \"Translated\", " +
            "repeats as \"Repeats\", " +
            "created_at as \"CreatedAt\", " +
            "languages.id, " +
            "languages.name as \"Name\", " +
            "users.id, " +
            "users.email as \"Email\", " +
            "users.phone as \"Phone\" " +
            "FROM user_words " +
            "INNER JOIN languages ON languages.id = user_words.language_id " +
            "INNER JOIN users ON users.id = user_words.user_id ";
    public readonly static string GetRange = Get +
        "LIMIT @Count";
    public readonly static string GetById = Get +
        "WHERE user_words.id = @Id";
    public readonly static string GetByWord = Get +
        "WHERE word = @Word";
    public readonly static string GetByUserId = Get +
        "WHERE user_id = @Id";
    public readonly static string GetCountPerDay =
        "SELECT COUNT(*) FROM user_words " +
        "WHERE user_id = @Id AND DATE(created_at) = @CreatedAt";
    public readonly static string GetMostRepeated = Get +
        "WHERE repeats = (SELECT MAX(repeats) FROM user_words WHERE user_id = @UserId) AND user_id = @UserId";
    public readonly static string GetRecordsByRepeatsAsc =
        "SELECT user_id as \"UserId\", " +
        "SUM(repeats) as \"Count\" " +
        "FROM user_words " +
        "GROUP BY (user_id) order by SUM(repeats) ASC " +
        "LIMIT @Count";
    public readonly static string GetRecordsByRepeatsDesc =
        "SELECT user_id as \"UserId\", " +
        "SUM(repeats) as \"Count\" " +
        "FROM user_words " +
        "GROUP BY (user_id) order by SUM(repeats) DESC " +
        "LIMIT @Count";
    public readonly static string GetRecordsByWordsCountAsc =
        "SELECT user_id as \"UserId\", " +
        "COUNT(word) as \"Count\" " +
        "FROM user_words " +
        "GROUP BY (user_id) order by COUNT(word) ASC " +
        "LIMIT @Count";
    public readonly static string GetRecordsByWordsCountDesc =
        "SELECT user_id as \"UserId\", " +
        "COUNT(word) as \"Count\" " +
        "FROM user_words " +
        "GROUP BY (user_id) order by COUNT(word) DESC " +
        "LIMIT @Count";
    public readonly static string Create =
        "INSERT INTO user_words (id, word, translated, repeats, created_at, language_id, user_id) " +
        "VALUES (@Id, @Word, @Translated, @Repeats, @CreatedAt, @LanguageId, @UserId)";
    public readonly static string Update =
        "UPDATE user_words " +
        "SET word = @Word, translated = @Translated, repeats = @Repeats, " +
        "created_at = @CreatedAt, language_id = @LanguageId, user_id = @UserId " +
        "WHERE id = @WordId";
    public readonly static string AddRepeats =
        "UPDATE user_words " +
        "SET repeats = repeats + @Repeats " +
        "WHERE id = @WordId";
    public readonly static string Delete =
        "DELETE FROM user_words " +
        "WHERE id = @Id";
}
