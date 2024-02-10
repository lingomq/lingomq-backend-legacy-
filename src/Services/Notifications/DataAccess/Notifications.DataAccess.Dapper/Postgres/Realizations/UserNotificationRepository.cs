using Dapper;
using Notifications.DataAccess.Dapper.Contracts;
using Notifications.DataAccess.Dapper.Postgres.RawQueries;
using Notifications.DataAccess.Dapper.Utils;
using Notifications.Domain.Entities;
using System.Data;

namespace Notifications.DataAccess.Dapper.Postgres.Realizations;
public class UserNotificationRepository : GenericRepository<UserNotification>, IUserNotificationRepository
{
    private readonly IDbConnection _connection;
    public UserNotificationRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;

    public async Task<UserNotification?> GetAsync(Guid id) =>
        await QueryFirstAsync(UserNotificationQueries.GetById, new { Id = id });

    public async Task<List<UserNotification>> GetAsync(int count) =>
        await QueryListAsync(UserNotificationQueries.GetRange, new { Count = count });

    public async Task CreateAsync(UserNotification entity) =>
        await ExecuteByTemplateAsync(UserNotificationQueries.Create, entity);

    public async Task UpdateAsync(UserNotification entity) =>
        await ExecuteByTemplateAsync(UserNotificationQueries.Update, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserNotificationQueries.Delete, new { Id = id });

    public async Task<List<UserNotification>> GetByUserIdAsync(Guid id) =>
        await QueryListAsync(UserNotificationQueries.GetByUserId, new { Id = id });

    public async Task<List<UserNotification>> GetByDateTimeRangeAsync(Guid userId, DateTime start, DateTime stop) =>
        await QueryListAsync(UserNotificationQueries.GetByDateTimeRange,
            new { Start = start, Finish = stop, Id = userId });

    public async Task<List<UserNotification>> GetUnreadAsync(Guid id) =>
        await QueryListAsync(UserNotificationQueries.GetUnread, new { Id = id });

    public async Task MarkAsReadAsync(Guid id) =>
        await ExecuteByTemplateAsync(UserNotificationQueries.MarkAsRead, new { Id = id });

    protected override async Task<UserNotification?> QueryFirstAsync<E>(string sql, E entity)
    {
        List<UserNotification> userNotifications = await QueryListAsync(sql, entity);
        return userNotifications.Any() ? userNotifications.FirstOrDefault() : null;
    }

    protected override async Task<List<UserNotification>> QueryListAsync<E>(string sql, E entity)
    {
        IEnumerable<UserNotification> values = await _connection.QueryAsync<UserNotification,
            User, Notification, NotificationType, UserNotification>(
            sql,
            (userNotification, user, notification, type) =>
            {
                userNotification.User = user;
                notification.NotificationType = type;
                userNotification.Notification = notification;

                return userNotification;
            }
            , entity, splitOn: "id");

        return values.Any() ? values.ToList() : new List<UserNotification>();
    }
}
