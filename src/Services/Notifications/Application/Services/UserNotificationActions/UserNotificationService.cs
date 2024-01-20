using Notifications.DataAccess.Dapper.Contracts;
using Notifications.Domain.Contracts;
using Notifications.Domain.Entities;
using Notifications.Domain.Exceptions.ClientExceptions;

namespace Notifications.Application.Services.UserNotificationActions;
public class UserNotificationService : IUserNotificationService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserNotificationService(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    public async Task<List<UserNotification>> GetAsync(Guid id)
    {
        List<UserNotification> notifications = await _unitOfWork.UserNotifications.GetByUserIdAsync(id);
        return notifications;
    }

    public async Task UpdateAsync(Guid id)
    {
        UserNotification? notification = await _unitOfWork.UserNotifications.GetAsync(id);
        if (notification is null)
            throw new NotFoundException<UserNotification>();
        if (notification.UserId != id) throw new ForbiddenException<User>("Access denied");

        await _unitOfWork.UserNotifications.MarkAsReadAsync(id);
    }
}
