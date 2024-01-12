using Identity.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.Application.Services.Jwt.Extensions;
public static class JwtExtensions
{

    public static string GenerateEmailToken(this IJwtService jwtService, AuthenticationModel model)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, model.Email!),
            new Claim(ClaimTypes.Name, model.Nickname!),
            new Claim(ClaimTypes.Authentication, model.Password!),
            new Claim(ClaimTypes.Version, "email")
        };

        DateTime expiration = DateTime.Now.AddMinutes(60);
        JwtSecurityToken jwtEmailToken = jwtService.CreateToken(claims, expiration);

        return jwtService.WriteToken(jwtEmailToken);
    }
}
