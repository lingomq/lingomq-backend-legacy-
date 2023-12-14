using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.MassTransit.Consumers.IdentityConsumers;

public class TopicsDeleteUserConsumer : IConsumer<IdentityModelDeleteUser>
{
    private readonly ILogger<TopicsDeleteUserConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public TopicsDeleteUserConsumer(ILogger<TopicsDeleteUserConsumer> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<IdentityModelDeleteUser> context)
    {
        Guid id = context.Message.Id;
        User? user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user is null)
            _logger.LogError("[-] [Topics UserDelete Consumer] " +
                             "Failed: User not found");

        await _unitOfWork.Users.DeleteAsync(id);

        _logger.LogInformation("[+] [Topics UserDelete Consumer] " +
                               "Success: User has been deleted");
    }
}

