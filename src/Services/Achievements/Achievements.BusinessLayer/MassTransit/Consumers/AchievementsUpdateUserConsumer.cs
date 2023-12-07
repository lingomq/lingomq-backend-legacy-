using Achievements.BusinessLayer.Contracts;
using Achievements.DomainLayer.Entities;
using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Achievements.BusinessLayer.MassTransit.Consumers
{
    public class AchievementsUpdateUserConsumer : IConsumer<IdentityModelUpdateUser>
    {
        private readonly ILogger<AchievementsUpdateUserConsumer> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public AchievementsUpdateUserConsumer(ILogger<AchievementsUpdateUserConsumer> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }


        public async Task Consume(ConsumeContext<IdentityModelUpdateUser> context)
        {
            User user = new User()
            {
                Id = context.Message.Id,
                Email = context.Message.Email,
                Phone = context.Message.Phone
            };

            await _unitOfWork.Users.UpdateAsync(user);

            _logger.LogInformation("[+] [Achievements Consumer] Succesfully updated");
        }
    }
}
