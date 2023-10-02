using Identity.DomainLayer.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.UnitTests.Common.Services
{
    internal class JwtService
    {
        private IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateTokenPair(UserInfo userInfo)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var accessIdentity = GenerateAccessTokenClaims(userInfo);

            JwtSecurityToken accessToken = CreateToken(accessIdentity,
                DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:ExpiredMinutesAccessToken"]!)));


            return tokenHandler.WriteToken(accessToken);
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
    }
}
