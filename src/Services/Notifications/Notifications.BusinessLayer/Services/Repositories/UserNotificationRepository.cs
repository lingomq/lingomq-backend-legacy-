using System.Data;
using Dapper;
using Notifications.BusinessLayer.Contracts;
using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.Services.Repositories;

public class UserNotificationRepository : GenericRepository<UserNotification>, IUserNotificationRepository
{
    private static readonly string Get = "SELECT user_notifications.id, " +
                                         "date_of_receipt as \"DateOfReceipt\", " +
                                         "is_readed as \"IsReaded\", " +
                                         "users.id, " +
                                         "users.email as \"Email\", " +
                                         "users.phone as \"Phone\"," +
                                         "notifications.id," +
                                         "notifications.title as \"Title\"," +
                                         "notifications.content as \"Content\"," +
                                         "notification_types.id, " +
                                         "notification_types.name as \"Name\" " +
                                         "FROM user_notifications " +
                                         "JOIN users ON users.id = user_notifications.user_id " +
                                         "JOIN notifications ON notifications.id = user_notifications.notification_id " +
                                         "JOIN notification_types ON notifications_types.id = notifications.notification_type_id ";

    private static readonly string GetById = Get + "WHERE user_notifications.id = @Id";
    private static readonly string GetRange = Get + "LIMIT @Count";
    private static readonly string GetByUserId = Get + "WHERE users.id = @Id";
    private static readonly string GetUnread = Get + "WHERE users.id = @Id AND user_notifications.is_readed = false";
    private static readonly string GetByDateTimeRange =
        Get + "WHERE date_of_receipt > @Start AND date_of_receipt < @Finish";
    private static readonly string Create = "INSERT INTO user_notifications " +
                                            "(id, user_id, notification_id, date_of_receipt, is_readed) " +
                                            "VALUES (@Id, @UserId, @NotificationId, @DateOfReceipt, @IsReaded) ";

    private static readonly string Update = "UPDATE user_notifications SET " +
                                            "user_id = @UserId, " +
                                            "notification_id = @NotificationId, " +
                                            "date_of_receipt = @DateOfReceipt," +
                                            "is_readed = @IsReaded " +
                                            "WHERE id = @Id";

    private static readonly string MarkAsRead = "UPDATE user_notifications SET " +
                                                "is_readed = TRUE " +
                                                "WHERE id = @Id";
    
    private static readonly string Delete = "DELETE FROM user_notifications WHERE id = @Id";
    private readonly IDbConnection _connection;

    protected UserNotificationRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;

    public async Task<UserNotification?> GetAsync(Guid id)
    {
        List<UserNotification> notifications = await QueryAsync(GetRange, new { Id = id });
        return notifications.FirstOrDefault();
    }

    public async Task<List<UserNotification>> GetAsync(int range) => await QueryAsync(GetRange, new { Count = range });

    public async Task UpdateAsync(UserNotification entity) => await ExecuteAsync(Update, entity);

    public async Task DeleteAsync(Guid id) => await ExecuteAsync(Delete, new { Id = id });

    public async Task CreateAsync(UserNotification entity) => await ExecuteAsync(Create, entity);

    public async Task<List<UserNotification>> GetByUserIdAsync(Guid id) =>
        await QueryAsync(GetByUserId, new { Id = id });

    public async Task<List<UserNotification>> GetByDateTimeRangeAsync(Guid userId, DateTime start, DateTime finish) =>
        await QueryAsync(GetByDateTimeRange, new { Start = start, Finish = finish });

    public async Task<List<UserNotification>> GetUnreadAsync(Guid id) => await QueryAsync(GetUnread, new { Id = id });
    public async Task MarkAsReadAsync(Guid id) => await ExecuteAsync(MarkAsRead, new { Id = id });

    protected override async Task<List<UserNotification>> QueryAsync<TE>(string sql, TE entity)
    {
        IEnumerable<UserNotification> values = await _connection.QueryAsync<UserNotification,
            User, Notification, NotificationType , UserNotification>(
            sql,
            (userNotification, user, notification, type) =>
            {
                userNotification.User = user;
                notification.NotificationType = type;
                userNotification.Notification = notification;
                
                return userNotification;
            }
            , entity, splitOn: "id");

        return values.ToList();
    }
}