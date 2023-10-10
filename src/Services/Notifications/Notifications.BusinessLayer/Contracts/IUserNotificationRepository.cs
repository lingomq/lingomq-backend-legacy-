using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.Contracts;

public interface IUserNotificationRepository : IGenericRepository<UserNotification>
{
    Task<List<UserNotification>> GetByUserId(Guid id);
    Task<List<UserNotification>> GetByDateTimeRange(Guid userId, DateTime start, DateTime finish);
    Task<List<UserNotification>> GetUnread(Guid id);
}