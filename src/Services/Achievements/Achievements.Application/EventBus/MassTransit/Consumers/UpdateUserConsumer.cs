using Achievements.DataAccess.Dapper.Contracts;
using Achievements.Domain.Entities;
using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Achievements.Application.EventBus.MassTransit.Consumers;
public class UpdateUserConsumer : IConsumer<IdentityModelUpdateUser>
{
    private readonly ILogger<UpdateUserConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateUserConsumer(ILogger<UpdateUserConsumer> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<IdentityModelUpdateUser> context)
    {
        User user = new User()
        {
            Id = context.Message.Id,
            Email = context.Message.Email,
            Phone = context.Message.Phone
        };

        await _unitOfWork.Users.UpdateAsync(user);

        _logger.LogInformation("[+] [Achievements Consumer] Succesfully updated");
    }
}
