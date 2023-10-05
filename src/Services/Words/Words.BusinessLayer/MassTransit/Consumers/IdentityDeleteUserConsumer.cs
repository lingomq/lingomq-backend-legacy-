using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using Words.BusinessLayer.Contracts;
using Words.BusinessLayer.Dtos;

namespace Words.BusinessLayer.MassTransit.Consumers
{
    public class IdentityDeleteUserConsumer : IConsumer<IdentityModelDeleteUser>
    {
        private readonly ILogger<IdentityDeleteUserConsumer> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public IdentityDeleteUserConsumer(ILogger<IdentityDeleteUserConsumer> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<IdentityModelDeleteUser> context)
        {
            Guid id = context.Message.Id;
            UserDto? user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user is null)
                _logger.LogError("[-] [Authentication UserDelete Consumer] " +
                                 "Failed: User not found");

            await _unitOfWork.Users.DeleteAsync(id);

            _logger.LogInformation("[+] [Authentication UserDelete Consumer] " +
                                   "Success: User has been deleted");
        }
    }
}
