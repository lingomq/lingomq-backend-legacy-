namespace Notifications.DataAccess.Dapper.Postgres.RawQueries;
public static class NotificationTypeQueries
{
    public static readonly string Get = "SELECT id as \"Id\", " +
                                         "name as \"Name\" " +
                                         "FROM notification_types ";

    public static readonly string GetById = Get + "WHERE id = @Id";
    public static readonly string GetRange = Get + "LIMIT @Count";
    public static readonly string Create = "INSERT INTO notification_types " +
                                            "(id, name) " +
                                            "VALUES (@Id, @Name)";

    public static readonly string Update = "UPDATE notification_types SET" +
                                            "name = @Name WHERE id = @Id";
    public static readonly string Delete = "DELETE FROM notification_types WHERE id = @Id";
}
