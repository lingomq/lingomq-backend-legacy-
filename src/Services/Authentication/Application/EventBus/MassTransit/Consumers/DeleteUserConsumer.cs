using Authentication.DataAccess.Dapper.Contracts;
using Authentication.Domain.Entities;
using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Authentication.Application.EventBus.MassTransit.Consumers;
public class DeleteUserConsumer : IConsumer<IdentityModelDeleteUser>
{
    private readonly ILogger<DeleteUserConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserConsumer(ILogger<DeleteUserConsumer> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<IdentityModelDeleteUser> context)
    {
        Guid id = context.Message.Id;
        User? user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user is null)
            _logger.LogError("[-] [Authentication UserDelete Consumer] " +
                             "Failed: User not found");

        await _unitOfWork.Users.DeleteAsync(id);

        _logger.LogInformation("[+] [Authentication UserDelete Consumer] " +
                               "Success: User has been deleted");
    }
}
