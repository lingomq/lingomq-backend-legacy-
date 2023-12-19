using Notifications.Domain.Entities;

namespace Notifications.Domain.Contracts;
public interface INotificationService
{
    Task<NotificationType> GetAsync(int count);
    Task<NotificationType> GetAsync(Guid id);
    Task CreateAsync(NotificationType notificationType);
    Task UpdateAsync(NotificationType notificationType);
    Task DeleteAsync(Guid id);
}
