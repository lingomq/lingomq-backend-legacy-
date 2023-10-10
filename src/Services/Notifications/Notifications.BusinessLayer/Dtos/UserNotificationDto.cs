namespace Notifications.BusinessLayer.Dtos;

public class UserNotificationDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid NotificationId { get; set; }
    public DateTime DateOfReceipt { get; set; }
    public bool IsReaded { get; set; }
}