using System.Text.Json.Serialization;

namespace Notifications.Domain.Entities;
public class Notification : EntityBase
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    [JsonIgnore]
    public Guid NotificationTypeId { get; set; }
    public NotificationType? NotificationType { get; set; }
}
