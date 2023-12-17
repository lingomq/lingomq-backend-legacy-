using EventBus.Entities.Identity.UserInfo;
using Identity.Domain.Entities;

namespace Domain.Contracts.Extensions;
public static class IdentityModelUpdateUserInfoExtension
{
    public static IdentityModelUpdateUserInfo ToConsume(this IdentityModelUpdateUserInfo identityModel, UserInfo info)
    {
        return new IdentityModelUpdateUserInfo()
        {
            Id = info.Id,
            Nickname = info.Nickname,
            Additional = info.Additional,
            ImageUri = info.ImageUri,
            RoleId = info.RoleId,
            UserId = info.UserId,
            CreationalDate = info.CreationalDate,
            IsRemoved = info.IsRemoved
        };
    }
}
