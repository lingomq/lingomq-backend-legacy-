using Authentication.BusinessLayer.Contracts;
using Authentication.DomainLayer.Entities;
using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Authentication.BusinessLayer.MassTransit.Consumers;

public class IdentityUpdateUserCredentialsConsumer : IConsumer<IdentityModelUpdateUserCredentials>
{
    private readonly ILogger<IdentityUpdateUserCredentialsConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public IdentityUpdateUserCredentialsConsumer(ILogger<IdentityUpdateUserCredentialsConsumer> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<IdentityModelUpdateUserCredentials> context)
    {
        User user = new User
        {
            Id = context.Message.Id,
            PasswordHash = context.Message.PasswordHash,
            PasswordSalt = context.Message.PasswordSalt
        };

        User? userFromDatabase = await _unitOfWork.Users.GetByGuidAsync(user.Id);

        if (userFromDatabase is null)
        {
            _logger.LogInformation("[-] [Authentication UpdateCredentials Consumer] " +
                                   "Failed: User not found");
            throw new InvalidDataException();
        }

        await _unitOfWork.Users.UpdateCredentials(user);
        _logger.LogInformation("[+] [Authentication UpdateCredentials Consumer]" +
                               "Success: User credentials has been updated");
    }
}