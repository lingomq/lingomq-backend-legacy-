using Authentication.BusinessLayer.Models;
using Authentication.BusinessLayer.Dtos;
using System.Security.Claims;

namespace Authentication.BusinessLayer.Contracts
{
    public interface IJwtService
    {
        public TokenModel CreateUserTokenPair(UserInfoDto userInfo);
        public List<Claim> GenerateRefreshTokenIdentity(UserInfoDto userInfo);
        public List<Claim> GenerateAccessTokenIdentity(UserInfoDto userInfo);
        public bool Validate(string refreshToken);
    }
}
