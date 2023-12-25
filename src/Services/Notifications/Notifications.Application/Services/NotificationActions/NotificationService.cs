using Notifications.DataAccess.Dapper.Contracts;
using Notifications.Domain.Contracts;
using Notifications.Domain.Entities;
using Notifications.Domain.Exceptions.ClientExceptions;

namespace Notifications.Application.Services.NotificationActions;
public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    public async Task<List<NotificationType>> GetAsync(int count)
    {
        List<NotificationType> types = await _unitOfWork.NotificationTypes.GetAsync(count);
        return types;
    }

    public async Task<NotificationType> GetAsync(Guid id)
    {
        NotificationType? type = await _unitOfWork.NotificationTypes.GetAsync(id);
        if (type is null)
            throw new NotFoundException<NotificationType>();

        return type;
    }

    public async Task CreateAsync(NotificationType notificationType)
    {
        await _unitOfWork.NotificationTypes.CreateAsync(notificationType);
    }

    public async Task UpdateAsync(NotificationType notificationType)
    {
        if (await _unitOfWork.NotificationTypes.GetAsync(notificationType.Id) is null)
            throw new InvalidDataException<NotificationType>(new[] { "id" });

        await _unitOfWork.NotificationTypes.UpdateAsync(notificationType);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (await _unitOfWork.NotificationTypes.GetAsync(id) is null)
            throw new InvalidDataException<NotificationType>(new[] { "id" });

        await _unitOfWork.NotificationTypes.DeleteAsync(id);
    }
}
