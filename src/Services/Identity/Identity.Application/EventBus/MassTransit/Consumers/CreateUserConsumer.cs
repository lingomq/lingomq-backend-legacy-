using DataAccess.Dapper.Contracts;
using EventBus.Entities.Identity.User;
using Identity.Application.EventBus.MassTransit.Extensions;
using Identity.Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Identity.Application.EventBus.MassTransit.Consumers;
public class CreateUserConsumer : IConsumer<IdentityModelCreateUser>
{
    private readonly ILogger<CreateUserConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public CreateUserConsumer(ILogger<CreateUserConsumer> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<IdentityModelCreateUser> context)
    {
        User user = new User();
        user.ToConsume(context);

        await _unitOfWork.Users.AddAsync(user);

        UserStatistics statistics = new UserStatistics();
        statistics.UserId = user.Id;

        await _unitOfWork.UserStatistics.AddAsync(statistics);
        UserRole? role = await _unitOfWork.UserRoles.GetByNameAsync(context.Message.RoleName!);

        UserInfo userInfo = new UserInfo();
        userInfo.ToConsume(user, role!, context);

        await _unitOfWork.UserInfos.AddAsync(userInfo);

        _logger.LogInformation("[+] [Identity Create Consumer] Succesfully get message." +
            "{0}:{1}:{2} has been added", user.Id, user.Email, userInfo.Nickname);
    }
}
