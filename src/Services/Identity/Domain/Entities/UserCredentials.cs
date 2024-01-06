namespace Identity.Domain.Entities;
public class UserCredentials : EntityBase
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
