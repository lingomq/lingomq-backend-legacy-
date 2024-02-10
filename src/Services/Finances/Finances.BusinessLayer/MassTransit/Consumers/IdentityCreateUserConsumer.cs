using EventBus.Entities.Identity.User;
using Finances.BusinessLayer.Contracts;
using Finances.DomainLayer.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Finances.BusinessLayer.MassTransit.Consumers;

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

        await _unitOfWork.Users.CreateAsync(user);

        _logger.LogInformation("[+] [Finances Create Consumer] Succesfully get message." +
                               "{0}:{1} has been added", user.Id, user.Email);
    }
}