﻿using Identity.Domain.Entities;
using Identity.Domain.Exceptions.ClientExceptions;
using Identity.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Application.Services.Jwt;
public class JwtService : IJwtService
{
    private IConfiguration _configuration;
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public TokenModel CreateTokenPair(UserInfo userInfo)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        var accessIdentity = GenerateAccessTokenClaims(userInfo);
        var refreshIdentity = GenerateRefreshTokenClaims(userInfo);

        JwtSecurityToken accessToken = CreateToken(accessIdentity,
            DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:ExpiredMinutesAccessToken"]!)));

        JwtSecurityToken refreshToken = CreateToken(refreshIdentity,
            DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:ExpiredMinutesRefreshToken"]!)));

        return new TokenModel()
        {
            AccessToken = tokenHandler.WriteToken(accessToken),
            RefreshToken = tokenHandler.WriteToken(refreshToken),
            AccessExpiredAt = DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:ExpiredMinutesAccessToken"]!))
        };
    }
    public List<Claim> GenerateRefreshTokenClaims(UserInfo userInfo)
    {
        List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserId!.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userInfo.Role!.Name!),
                new Claim(ClaimTypes.Email, userInfo.User!.Email!),
                new Claim(ClaimTypes.Name, userInfo.Nickname!),
                new Claim(ClaimTypes.Version, "refresh")
            };

        return claims;
    }
    public List<Claim> GenerateAccessTokenClaims(UserInfo userInfo)
    {
        List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, new Guid(userInfo.UserId!.ToString()).ToString()),
                new Claim(ClaimTypes.Name, userInfo.Nickname!),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userInfo.Role!.Name!),
                new Claim(ClaimTypes.Version, "access")
            };

        return claims;
    }
    public string WriteToken(JwtSecurityToken token) =>
        new JwtSecurityTokenHandler().WriteToken(token);

    public JwtSecurityToken CreateToken(List<Claim> claims, DateTime expires)
    {
        var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!);
        return new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256)
        );
    }
    public ClaimsPrincipal GetClaimsPrincipal(string token)
    {
        if (token.IsNullOrEmpty())
            throw new InvalidTokenException<User>("Токен неверный");

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!);

        try
        {
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken securityToken);

            JwtSecurityToken jwtToken = (JwtSecurityToken)securityToken;

            if (securityToken is null ||
                !jwtToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Токен неверный");

            return principal;
        }
        catch
        {
            throw new InvalidTokenException<User>("Токен неверный");
        }
    }
}
