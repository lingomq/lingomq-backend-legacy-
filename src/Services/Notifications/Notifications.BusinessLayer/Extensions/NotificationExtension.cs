using Notifications.BusinessLayer.Dtos;
using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.Extensions;

public static class NotificationExtension
{
    public static NotificationDto ToDto(this Notification notification) =>
        new()
        {
            Id = notification.Id,
            Title = notification.Title,
            Content = notification.Content,
            NotificationId = notification.NotificationId
        };

    public static Notification ToModel(this NotificationDto dto) =>
        new()
        {
            Id = dto.Id,
            Title = dto.Title,
            Content = dto.Content,
            NotificationId = dto.NotificationId
        };
}