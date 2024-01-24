using EventBus.Entities.Identity.User;
using Identity.Domain.Entities;

namespace Identity.Application.Services.Confirm.Extensions;
public static class IdentityModelCreateUserExtension
{
    public static IdentityModelCreateUser SetValues(this IdentityModelCreateUser model, User user, UserInfo info, UserRole role)
    {
        return new IdentityModelCreateUser()
        {
            UserId = user.Id,
            InfoId = info.Id,
            Nickname = info.Nickname,
            Email = user.Email,
            Phone = user.Phone,
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt,
            RoleName = role!.Name
        };
    }
}
