using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.MassTransit.Consumers.IdentityConsumers;

public class IdentityCreateUserConsumer : IConsumer<IdentityModelCreateUser>
{
    private readonly ILogger<IdentityCreateUserConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public IdentityCreateUserConsumer(ILogger<IdentityCreateUserConsumer> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<IdentityModelCreateUser> context)
    {
        User user = new User()
        {
            Id = context.Message.UserId,
            Email = context.Message.Email,
            Phone = context.Message.Phone
        };

        await _unitOfWork.Users.AddAsync(user);

        _logger.LogInformation("[+] [Topics Create Consumer] Succesfully get message." +
                               "{0}:{1} has been added", user.Id, user.Email);
    }
}