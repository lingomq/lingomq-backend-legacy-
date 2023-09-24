using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IdentityModel.Tokens.Jwt;

namespace Authentication.BusinessLayer.Extensions
{
    public static class JwtSecurityTokenExtension
    {
        public static string ToString(this JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
