using Authentication.BusinessLayer.Models;
using Authentication.BusinessLayer.Dtos;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Authentication.BusinessLayer.Contracts
{
    public interface IJwtService
    {
        public TokenModel CreateTokenPair(UserInfoDto userInfo);
        public List<Claim> GenerateRefreshTokenClaims(UserInfoDto userInfo);
        public List<Claim> GenerateAccessTokenClaims(UserInfoDto userInfo);
        public JwtSecurityToken CreateToken(List<Claim> claims, DateTime expires);
        public ClaimsPrincipal GetClaimsPrincipal(string token);
    }
}
