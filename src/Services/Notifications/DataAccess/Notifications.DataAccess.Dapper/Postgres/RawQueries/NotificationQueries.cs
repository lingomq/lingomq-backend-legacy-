namespace Notifications.DataAccess.Dapper.Postgres.RawQueries;
public static class NotificationQueries
{
    public static readonly string Get = "SELECT notifications.id," +
                                         "title as \"Title\", " +
                                         "content as \"Content\", " +
                                         "notification_types.id, " +
                                         "notification_types.name as \"Name\" " +
                                         "FROM notifications " +
                                         "JOIN notification_types ON notification_types.id = notifications.notification_type_id ";
    public static readonly string GetById = Get + "WHERE notifications.id = @Id";
    public static readonly string GetRange = Get + "LIMIT @Count";
    public static readonly string Create = "INSERT INTO notifications " +
                                            "(id, title, content, notification_type_id) " +
                                            "VALUES (@Id, @Title, @Content, @NotificationTypeId)";
    public static readonly string Update = "UPDATE notifications SET " +
                                            "title = @Title, " +
                                            "content = @Content, " +
                                            "notification_type_id = @NotificationTypeId";
    public static readonly string Delete = "DELETE FROM notifications " +
                                            "WHERE id = @Id";
}
