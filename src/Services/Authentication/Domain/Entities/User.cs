namespace Authentication.Domain.Entities;

public class User : EntityBase
{
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
}

