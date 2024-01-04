namespace DataAccess.Dapper.Postgres.RawQueries;
public static class UserWordTypeQueries
{
    public readonly static string Get =
        "SELECT " +
        "user_words.id, " +
        "user_words.word as \"Word\", " +
        "user_words.translated as \"Translated\", " +
        "user_words.repeats as \"Repeats\", " +
        "user_words.created_at as \"CreatedAt\", " +
        "languages.id, " +
        "languages.name as \"Name\", " +
        "users.id, " +
        "users.email as \"Email\", " +
        "users.phone as \"Phone\", " +
        "word_types.id, " +
        "word_types.type_name as \"TypeName\" " +
        "FROM user_word_types " +
        "INNER JOIN user_words ON user_words.id = user_word_types.user_word_id " +
        "INNER JOIN languages ON languages.id = user_words.language_id " +
        "INNER JOIN users ON users.id = user_words.user_id " +
        "INNER JOIN word_types ON word_types.id = user_word_types.word_type_id ";
    public readonly static string GetRange = Get +
        "LIMIT @Count";
    public readonly static string GetById = Get +
        "WHERE user_word_types.id = @Id";
    public readonly static string GetByUserId = Get +
        "WHERE users.id = @Id";
    public readonly static string GetByTypeId = Get +
        "WHER word_types.id = @Id";
    public readonly static string Create =
        "INSERT INTO user_word_types (id, user_word_id, word_type_id) " +
        "VALUES (@Id, @UserWordId, @WordTypeId)";
    public readonly static string Update =
        "UPDATE user_word_types " +
        "SET user_word_id = @UserWordId, word_type_id = @WordTypeId " +
        "WHERE id = @Id";
    public readonly static string Delete =
        "DELETE FROM user_word_types " +
        "WHERE id = @Id";
}
