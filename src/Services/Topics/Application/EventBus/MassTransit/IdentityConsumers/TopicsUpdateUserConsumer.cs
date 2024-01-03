using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.MassTransit.Consumers.IdentityConsumers;

public class TopicsUpdateUserConsumer: IConsumer<IdentityModelUpdateUser>
{
    private readonly ILogger<TopicsUpdateUserConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public TopicsUpdateUserConsumer(ILogger<TopicsUpdateUserConsumer> logger, IUnitOfWork unitOfWork)
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

        _logger.LogInformation("[+] [Topics Consumer] Succesfully updated");
    }
}

