using Identity.Domain.Entities;
using Identity.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.Application.Services.Jwt;
public interface IJwtService
{
    public TokenModel CreateTokenPair(UserInfo userInfo);
    public List<Claim> GenerateRefreshTokenClaims(UserInfo userInfo);
    public List<Claim> GenerateAccessTokenClaims(UserInfo userInfo);
    public JwtSecurityToken CreateToken(List<Claim> claims, DateTime expires);
    public ClaimsPrincipal GetClaimsPrincipal(string token);
    public string WriteToken(JwtSecurityToken token);
}
