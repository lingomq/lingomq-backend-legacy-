using Authentication.Domain.Entities;
using System.Security.Claims;

namespace Authentication.Application.Services.Confirm.Extensions;
public static class UserInfoExtension
{
    public static UserInfo SetUserInfo(this UserInfo userInfo, User user, ClaimsPrincipal principal, UserRole role)
    {
        UserInfo info = new UserInfo()
        {
            Id = Guid.NewGuid(),
            Nickname = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value,
            RoleId = role!.Id,
            Role = role,
            UserId = user.Id,
            User = user
        };
        userInfo = info;
        userInfo.Role.Name = info.Role.Name;
        userInfo.User.Email = info.User.Email;
        userInfo.User.Phone = info.User.Phone;
        userInfo.User.PasswordHash = info.User.PasswordHash;
        userInfo.User.PasswordSalt = info.User.PasswordSalt;

        return userInfo;
    }
}
