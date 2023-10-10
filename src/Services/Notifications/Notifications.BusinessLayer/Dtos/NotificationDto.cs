namespace Notifications.BusinessLayer.Dtos;

public class NotificationDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public Guid NotificationId { get; set; }
}