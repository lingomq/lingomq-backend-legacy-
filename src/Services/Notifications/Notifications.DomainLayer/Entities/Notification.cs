using System.Text.Json.Serialization;

namespace Notifications.DomainLayer.Entities;

public class Notification : BaseEntity
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    [JsonIgnore]
    public Guid NotificationId { get; set; }
    public NotificationType? NotificationType { get; set; }
}