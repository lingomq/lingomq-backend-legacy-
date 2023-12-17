namespace DataAccess.Dapper.Postgres.RawQueries;
public static class UserQueries
{
    public readonly static string Get =
        "SELECT id as \"Id\", email as \"Email\", phone as \"Phone\", " +
        "password_hash as \"PasswordHash\", password_salt as \"PasswordSalt\"" +
        "FROM users";
    public readonly static string GetWithoutCredentials =
        "SELECT id as \"Id\", email as \"Email\", phone as \"Phone\" " +
        "FROM users";
    public readonly static string GetRange = Get +
        " LIMIT @Count";
    public readonly static string GetRangeWithoutCredentials = GetWithoutCredentials +
        " LIMIT @Count";
    public readonly static string GetById = Get +
        " WHERE id = @Id";
    public readonly static string GetByIdWithoutCredentials = GetWithoutCredentials +
        " WHERE id = @Id";
    public readonly static string GetByEmail = GetWithoutCredentials +
        " WHERE email = @Email";
    public readonly static string Create =
        "INSERT INTO users (id, email, phone, password_hash, password_salt) " +
        "VALUES (@Id, @Email, @Phone, @PasswordHash, @PasswordSalt)";
    public readonly static string Delete =
        "DELETE FROM users " +
        "WHERE id = @Id";
    public readonly static string Update =
        "UPDATE users " +
        "SET " +
        "email = @Email," +
        "phone = @Phone " +
        "WHERE id = @Id";
    public readonly static string UpdateCredentials =
        "UPDATE users " +
        "SET " +
        "password_hash = @PasswordHash," +
        "password_salt = @PasswordSalt " +
        "WHERE id = @Id";
}
