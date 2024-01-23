using EventBus.Entities.Identity.User;
using Identity.Domain.Entities;
using MassTransit;

namespace Identity.Application.EventBus.MassTransit.Extensions;
public static class UserInfoExtension
{
    public static UserInfo ToConsume(this UserInfo userInfo, User user, UserRole role, ConsumeContext<IdentityModelCreateUser> context)
    {
        userInfo.Id = context.Message.InfoId;
        userInfo.Nickname = context.Message.Nickname;
        userInfo.RoleId = role!.Id;
        userInfo.Additional = "";
        userInfo.IsRemoved = false;
        userInfo.CreationalDate = DateTime.UtcNow;
        userInfo.UserId = user.Id;

        return userInfo;
    }
}
