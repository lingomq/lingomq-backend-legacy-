namespace Notifications.DomainLayer.Entities;

public class User : BaseEntity
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
}