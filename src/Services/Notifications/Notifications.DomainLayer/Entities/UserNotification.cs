using System.Text.Json.Serialization;

namespace Notifications.DomainLayer.Entities;

public class UserNotification : BaseEntity
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public Guid NotificationId { get; set; }
    public User? User { get; set; }
    public Notification? Notification { get; set; }
    public DateTime DateOfReceipt { get; set; }
    public bool IsReaded { get; set; }
}