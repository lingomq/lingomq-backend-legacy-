using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notifications.BusinessLayer.Contracts;
using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.MassTransit.Consumers;

public class NotificationsUpdateUserConsumer: IConsumer<IdentityModelUpdateUser>
{
    private readonly ILogger<NotificationsUpdateUserConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public NotificationsUpdateUserConsumer(ILogger<NotificationsUpdateUserConsumer> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }


    public async Task Consume(ConsumeContext<IdentityModelUpdateUser> context)
    {
        User user = new User()
        {
            Id = context.Message.Id,
            Email = context.Message.Email,
            Phone = context.Message.Phone
        };

        await _unitOfWork.Users.UpdateAsync(user);

        List<NotificationType> notificationTypes = await _unitOfWork.NotificationTypes.GetAsync(10);
        NotificationType? importantType = notificationTypes.FirstOrDefault(x => x.Name == "important");

        Notification notification = new Notification()
        {
            Id = Guid.NewGuid(),
            Title = "You have changed your account details",
            Content = "You have changed your account details",
            NotificationTypeId = importantType!.Id
        };

        await _unitOfWork.Notifications.CreateAsync(notification);

        UserNotification userNotification = new UserNotification()
        {
            Id = Guid.NewGuid(),
            NotificationId = notification.Id,
            UserId = user.Id,
            DateOfReceipt = DateTime.Now,
            IsReaded = false
        };

        await _unitOfWork.UserNotifications.CreateAsync(userNotification);

        _logger.LogInformation("[+] [Topics Consumer] Succesfully updated");
    }
}

