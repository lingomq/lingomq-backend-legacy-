using EventBus.Entities.Identity.User;
using Identity.Domain.Entities;
using MassTransit;

namespace Identity.Application.EventBus.MassTransit.Extensions;
public static class UserInfoExtension
{
    public static UserInfo ToConsume(this UserInfo userInfo, User user, UserRole role, ConsumeContext<IdentityModelCreateUser> context)
    {
        return new UserInfo()
        {
            Id = context.Message.InfoId,
            Nickname = context.Message.Nickname,
            RoleId = role!.Id,
            Additional = "",
            IsRemoved = false,
            CreationalDate = DateTime.UtcNow,
            UserId = user.Id
        };
    }
}
