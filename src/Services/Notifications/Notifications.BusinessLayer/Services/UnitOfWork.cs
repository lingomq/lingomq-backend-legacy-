using Notifications.BusinessLayer.Contracts;

namespace Notifications.BusinessLayer.Services;

public class UnitOfWork : IUnitOfWork
{
    public INotificationRepository Notifications { get; }
    public INotificationTypeRepository NotificationTypes { get; }
    public IUserNotificationRepository UserNotifications { get; }
    public IUserRepository Users { get; }

    public UnitOfWork(INotificationRepository notificationRepository,
        INotificationTypeRepository notificationTypeRepository,
        IUserNotificationRepository userNotificationRepository,
        IUserRepository userRepository)
    {
        Notifications = notificationRepository;
        NotificationTypes = notificationTypeRepository;
        UserNotifications = userNotificationRepository;
        Users = userRepository;
    }
}