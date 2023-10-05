using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using Words.BusinessLayer.Contracts;
using Words.BusinessLayer.Dtos;

namespace Words.BusinessLayer.MassTransit.Consumers
{
    public class IdentityUpdateUserConsumer : IConsumer<IdentityModelUpdateUser>
    {
        private readonly ILogger<IdentityUpdateUserConsumer> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public IdentityUpdateUserConsumer(ILogger<IdentityUpdateUserConsumer> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }


        public async Task Consume(ConsumeContext<IdentityModelUpdateUser> context)
        {
            UserDto user = new UserDto()
            {
                Id = context.Message.Id,
                Email = context.Message.Email,
                Phone = context.Message.Phone
            };

            await _unitOfWork.Users.UpdateAsync(user);

            _logger.LogInformation("[+] [Authentication Consumer] Succesfully updated");
        }
    }
}
