namespace Authentication.Domain.Entities;

public class UserInfo : EntityBase
{
    public string? Nickname { get; set; }
    public string? ImageUri { get; set; } = "static/default.png";
    public string? Additional { get; set; } = "";
    public DateTime? CreationalDate { get; set; } = DateTime.UtcNow;
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }
    public bool IsRemoved { get; set; } = false;
    public UserRole? Role { get; set; }
    public User? User { get; set; }
}

