using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notifications.BusinessLayer.Contracts;
using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.MassTransit.Consumers;

public class NotificationsCreateUserConsumer : IConsumer<IdentityModelCreateUser>
{
    private readonly ILogger<NotificationsCreateUserConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public NotificationsCreateUserConsumer(ILogger<NotificationsCreateUserConsumer> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<IdentityModelCreateUser> context)
    {
        User user = new User()
        {
            Id = context.Message.UserId,
            Email = context.Message.Email,
            Phone = context.Message.Phone
        };

        await _unitOfWork.Users.CreateAsync(user);

        List<NotificationType> notificationTypes = await _unitOfWork.NotificationTypes.GetAsync(10);
        NotificationType? attentionType = notificationTypes.FirstOrDefault(x => x.Name == "attention");

        Notification notification = new Notification()
        {
            Id = Guid.NewGuid(),
            Title = "Welcome, " + context.Message.Nickname,
            Content = "Thank you for using our service to learn foreign words. " +
            "We hope you will get everything you need from our application",
            NotificationTypeId = attentionType!.Id
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

        _logger.LogInformation("[+] [Topics Create Consumer] Succesfully get message." +
                               "{0}:{1} has been added", user.Id, user.Email);
    }
}