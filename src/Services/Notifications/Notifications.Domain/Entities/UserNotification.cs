using System.Text.Json.Serialization;

namespace Notifications.Domain.Entities;
public class UserNotification : EntityBase
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public Guid NotificationId { get; set; }
    public User? User { get; set; }
    public Notification? Notification { get; set; }
    public DateTime DateOfReceipt { get; set; }
    public bool Readed { get; set; }
}
