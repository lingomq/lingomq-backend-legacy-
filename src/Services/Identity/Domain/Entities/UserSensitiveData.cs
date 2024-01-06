namespace Identity.Domain.Entities;
public class UserSensitiveData : EntityBase
{
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public Guid UserId { get; set; }
}
