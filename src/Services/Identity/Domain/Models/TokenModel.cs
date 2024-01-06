namespace Identity.Domain.Models;
public class TokenModel
{
    public DateTime AccessTokenExpires { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
