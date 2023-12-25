using Notifications.Domain.Entities;

namespace Notifications.DataAccess.Dapper.Contracts;
public interface IUserNotificationRepository : IGenericRepository<UserNotification>
{
    Task<List<UserNotification>> GetByUserIdAsync(Guid id);
    Task<List<UserNotification>> GetByDateTimeRangeAsync(Guid userId, DateTime start, DateTime stop);
    Task<List<UserNotification>> GetUnreadAsync(Guid id);
    Task MarkAsReadAsync(Guid id);
}
