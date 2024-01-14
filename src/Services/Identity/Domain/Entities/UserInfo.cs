namespace Identity.Domain.Entities;
public class UserInfo : EntityBase
{
    public DateTime? CreationTime { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; } = "";
    public string? Locale { get; set; } = "";
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
