using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.Contracts;

public interface IUserNotificationRepository : IGenericRepository<UserNotification>
{
    Task<List<UserNotification>> GetByUserIdAsync(Guid id);
    Task<List<UserNotification>> GetByDateTimeRangeAsync(Guid userId, DateTime start, DateTime finish);
    Task<List<UserNotification>> GetUnreadAsync(Guid id);
    Task MarkAsReadAsync(Guid id);
}