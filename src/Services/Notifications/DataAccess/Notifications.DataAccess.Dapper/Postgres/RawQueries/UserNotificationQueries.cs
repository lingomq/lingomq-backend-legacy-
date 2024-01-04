namespace Notifications.DataAccess.Dapper.Postgres.RawQueries;
public static class UserNotificationQueries
{
    public static readonly string Get = "SELECT user_notifications.id, " +
                                         "date_of_receipt as \"DateOfReceipt\", " +
                                         "is_readed as \"IsReaded\", " +
                                         "users.id, " +
                                         "users.email as \"Email\", " +
                                         "users.phone as \"Phone\", " +
                                         "notifications.id, " +
                                         "notifications.title as \"Title\", " +
                                         "notifications.content as \"Content\", " +
                                         "notification_types.id, " +
                                         "notification_types.name as \"Name\" " +
                                         "FROM user_notifications " +
                                         "JOIN users ON users.id = user_notifications.user_id " +
                                         "JOIN notifications ON notifications.id = user_notifications.notification_id " +
                                         "JOIN notification_types ON notification_types.id = notifications.notification_type_id ";

    public static readonly string GetById = Get + "WHERE user_notifications.id = @Id";
    public static readonly string GetRange = Get + "LIMIT @Count";
    public static readonly string GetByUserId = Get + "WHERE users.id = @Id";
    public static readonly string GetUnread = Get + "WHERE users.id = @Id AND user_notifications.is_readed = false";
    public static readonly string GetByDateTimeRange =
        Get + "WHERE date_of_receipt > @Start AND date_of_receipt < @Finish and user_id = @Id";
    public static readonly string Create = "INSERT INTO user_notifications " +
                                            "(id, user_id, notification_id, date_of_receipt, is_readed) " +
                                            "VALUES (@Id, @UserId, @NotificationId, @DateOfReceipt, @IsReaded) ";

    public static readonly string Update = "UPDATE user_notifications SET " +
                                            "user_id = @UserId, " +
                                            "notification_id = @NotificationId, " +
                                            "date_of_receipt = @DateOfReceipt," +
                                            "is_readed = @IsReaded " +
                                            "WHERE id = @Id";

    public static readonly string MarkAsRead = "UPDATE user_notifications SET " +
                                                "is_readed = TRUE " +
                                                "WHERE id = @Id";

    public static readonly string Delete = "DELETE FROM user_notifications WHERE id = @Id";
}
