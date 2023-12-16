using Microsoft.Extensions.Logging;
using Authentication.DataAccess.Dapper.Contracts;
using Authentication.Domain.Entities;
using EventBus.Entities.Identity.User;
using MassTransit;

namespace Authentication.Application.EventBus.MassTransit.Consumers;
public class UpdateUserCredentialsConsumer : IConsumer<IdentityModelUpdateUserCredentials>
{
    private readonly ILogger<UpdateUserCredentialsConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCredentialsConsumer(ILogger<UpdateUserCredentialsConsumer> logger,
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

        User? userFromDatabase = await _unitOfWork.Users.GetByIdAsync(user.Id);

        if (userFromDatabase is null)
        {
            _logger.LogInformation("[-] [Authentication UpdateCredentials Consumer] " +
                                   "Failed: User not found");
            throw new InvalidDataException();
        }

        await _unitOfWork.Users.UpdateCredentialsAsync(user);
        _logger.LogInformation("[+] [Authentication UpdateCredentials Consumer]" +
                               "Success: User credentials has been updated");
    }
}
