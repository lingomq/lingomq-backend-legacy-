using Identity.Domain.Entities;
using Identity.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.Application.Services.Jwt;
public interface IJwtService
{
    public TokenModel CreateTokenPair(User user, string email);
    public List<Claim> GenerateRefreshTokenClaims(User user, string email);
    public List<Claim> GenerateAccessTokenClaims(User user);
    public JwtSecurityToken CreateToken(List<Claim> claims, DateTime expires);
    public ClaimsPrincipal GetClaimsPrincipal(string token);
    public string WriteToken(JwtSecurityToken token);
}