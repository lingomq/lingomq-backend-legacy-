using Notifications.BusinessLayer.Dtos;
using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.Extensions;

public static class UserNotificationExtension
{
    public static UserNotificationDto ToDto(this UserNotification notification) =>
        new()
        {
            Id = notification.Id,
            UserId = notification.UserId,
            NotificationId = notification.NotificationId,
            DateOfReceipt = notification.DateOfReceipt,
            IsReaded = notification.IsReaded
        };
    
    public static UserNotification ToModel(this UserNotificationDto notificationDto) =>
        new()
        {
            Id = notificationDto.Id,
            UserId = notificationDto.UserId,
            NotificationId = notificationDto.NotificationId,
            DateOfReceipt = notificationDto.DateOfReceipt,
            IsReaded = notificationDto.IsReaded
        };
}