using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notifications.BusinessLayer.Contracts;
using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.MassTransit.Consumers;

public class NotificationsDeleteUserConsumer : IConsumer<IdentityModelDeleteUser>
{
    private readonly ILogger<NotificationsDeleteUserConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public NotificationsDeleteUserConsumer(ILogger<NotificationsDeleteUserConsumer> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<IdentityModelDeleteUser> context)
    {
        Guid id = context.Message.Id;
        User? user = await _unitOfWork.Users.GetAsync(id);

        if (user is null)
            _logger.LogError("[-] [Topics UserDelete Consumer] " +
                             "Failed: User not found");

        await _unitOfWork.Users.DeleteAsync(id);

        _logger.LogInformation("[+] [Topics UserDelete Consumer] " +
                               "Success: User has been deleted");
    }
}

