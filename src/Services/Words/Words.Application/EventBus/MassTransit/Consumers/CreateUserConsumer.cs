using DataAccess.Dapper.Contracts;
using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using Words.Domain.Entities;

namespace Words.Application.EventBus.MassTransit.Consumers;
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
        User user = new User()
        {
            Id = context.Message.UserId,
            Email = context.Message.Email,
            Phone = context.Message.Phone
        };

        await _unitOfWork.Users.AddAsync(user);
        _logger.LogInformation("[+] [Words Create Consumer] Succesfully get message." +
                               "{0}:{1} has been added", user.Id, user.Email);
    }
}
