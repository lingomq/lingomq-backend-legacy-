namespace Notifications.DomainLayer.Entities;

public class UserNotification : BaseEntity
{
    public User? User { get; set; }
    public Notification? Notification { get; set; }
    public DateTime DateOfReceipt { get; set; }
    public bool IsReaded { get; set; }
}