using Identity.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.Application.Services.Jwt;
public static class JwtExtensions
{
    public static string GenerateEmailToken(this IJwtService jwtService, SignModel model)
    {
        List<Claim> claims =  new List<Claim>()
        {
            new Claim(ClaimTypes.Email, model.Email!),
            new Claim(ClaimTypes.Name, model.Nickname!),
            new Claim(ClaimTypes.Authentication, model.Password!),
            new Claim(ClaimTypes.MobilePhone, model.Phone!),
            new Claim(ClaimTypes.Version, "email")
        };

        DateTime expiration = DateTime.Now.AddMinutes(600);
        JwtSecurityToken jwtEmailToken = jwtService.CreateToken(claims, expiration);

        return jwtService.WriteToken(jwtEmailToken);
    }
}
