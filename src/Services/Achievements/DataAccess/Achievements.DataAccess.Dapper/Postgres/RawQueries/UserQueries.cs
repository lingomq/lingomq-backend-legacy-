namespace Achievements.DataAccess.Dapper.Postgres.RawQueries;
public static class UserQueries
{
    public readonly static string Get =
        "SELECT id as \"Id\", email as \"Email\", phone as \"Phone\" " +
        "FROM users";
    public readonly static string GetRange = Get +
                                              " LIMIT @Count";
    public readonly static string GetById = Get +
                                             " WHERE id = @Id";
    public readonly static string GetByEmail = Get +
                                                " WHERE email = @Email";
    public readonly static string Create =
        "INSERT INTO users (id, email, phone) " +
        "VALUES (@Id, @Email, @Phone)";
    public readonly static string Delete =
        "DELETE FROM users " +
        "WHERE id = @Id";
    public readonly static string Update =
        "UPDATE users " +
        "SET " +
        "email = @Email," +
        "phone = @Phone " +
        "WHERE id = @Id";
}
