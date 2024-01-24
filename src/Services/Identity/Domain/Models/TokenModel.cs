﻿namespace Identity.Domain.Models;
public class TokenModel
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? AccessExpiredAt { get; set; }
}
