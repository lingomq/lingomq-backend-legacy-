using EventBus.Entities.Identity.User;
using Identity.Domain.Entities;
using MassTransit;

namespace Identity.Application.EventBus.MassTransit.Extensions;
public static class UserExtension
{
    public static User ToConsume(this User user, ConsumeContext<IdentityModelCreateUser> context)
    {
        user.Id = context.Message.UserId;
        user.Email = context.Message.Email;
        user.Phone = context.Message.Phone;
        user.PasswordHash = context.Message.PasswordHash;
        user.PasswordSalt = context.Message.PasswordSalt;

        return user;
    }
}
