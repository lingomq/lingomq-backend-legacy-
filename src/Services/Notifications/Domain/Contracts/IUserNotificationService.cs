using Notifications.Domain.Entities;

namespace Notifications.Domain.Contracts;
public interface IUserNotificationService
{
    Task<List<UserNotification>> GetAsync(Guid id);
    Task UpdateAsync(Guid id);
}
