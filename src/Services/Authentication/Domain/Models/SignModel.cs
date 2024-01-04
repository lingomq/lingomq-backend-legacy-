namespace Authentication.Domain.Models;

public class SignModel
{
    public string? Nickname { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; } = "";
    public string? Password { get; set; }
}

