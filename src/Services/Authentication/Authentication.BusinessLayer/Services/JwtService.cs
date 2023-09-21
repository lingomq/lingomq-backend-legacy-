using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Authentication.BusinessLayer.Contracts;
using Authentication.BusinessLayer.Models;
using Authentication.BusinessLayer.Dtos;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Authentication.BusinessLayer.Services
{
    public class JwtService : IJwtService
    {
        private IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TokenModel CreateUserTokenPair(UserInfoDto userInfo)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!);
            var accessIdentity = GenerateAccessTokenIdentity(userInfo);
            var refreshIdentity = GenerateRefreshTokenIdentity(userInfo);
            
            JwtSecurityToken accessToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: accessIdentity,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:ExpiredMinutesAccessToken"]!)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256)
            );
            
            JwtSecurityToken refreshToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: refreshIdentity,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:ExpiredMinutesRefreshToken"]!)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256)
            );

            return new TokenModel() { 
                AccessToken = tokenHandler.WriteToken(accessToken),
                RefreshToken = tokenHandler.WriteToken(refreshToken)    
            };
        }
        public List<Claim> GenerateRefreshTokenIdentity(UserInfoDto userInfo)
        {
            List<Claim> claims = new List<Claim>() 
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserId!),
                new Claim(ClaimTypes.Role, userInfo.Role!.Name!),
                new Claim(ClaimTypes.Email, userInfo.User!.Email!)
            };

            return claims;
        }
        public List<Claim> GenerateAccessTokenIdentity(UserInfoDto userInfo)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
                new Claim(ClaimTypes.Name, userInfo.Nickname!),
                new Claim(ClaimTypes.Role, userInfo.Role!.Name!)
            };

            return claims;   
        }
        public bool Validate(string refreshToken)
        {
            return false;
        }

    }
}
