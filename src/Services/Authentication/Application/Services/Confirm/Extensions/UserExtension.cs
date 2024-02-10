using Authentication.Domain.Entities;
using LingoMqCryptography.Entities;
using System.Security.Claims;

namespace Authentication.Application.Services.Confirm.Extensions;
public static class UserExtension
{
    public static void SetUserModel(this User user, ClaimsPrincipal principal, BaseKeyPair baseKeyPair)
    {
        user.Id = Guid.NewGuid();
        user.Email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
        user.Phone = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)!.Value;
        user.PasswordHash = baseKeyPair.Hash;
        user.PasswordSalt = baseKeyPair.Salt;
    }
}
