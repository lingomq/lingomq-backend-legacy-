namespace Notifications.DataAccess.Dapper.Contracts;
public interface IUnitOfWork
{
    public INotificationRepository Notifications { get; }
    public INotificationTypeRepository NotificationTypes { get; }
    public IUserNotificationRepository UserNotifications { get; }
    public IUserRepository Users { get; }
}
