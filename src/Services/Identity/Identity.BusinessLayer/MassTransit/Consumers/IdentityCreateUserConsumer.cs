using EventBus.Entities.Identity.User;
using Identity.BusinessLayer.Contracts;
using Identity.DomainLayer.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Identity.BusinessLayer.MassTransit.Consumers
{
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
                Phone = context.Message.Phone,
                PasswordHash = context.Message.PasswordHash,
                PasswordSalt = context.Message.PasswordSalt
            };

            await _unitOfWork.Users.AddAsync(user);

            UserStatistics statistics = new UserStatistics()
            {
                Id = Guid.NewGuid(),
                TotalHours = 0,
                TotalWords = 0,
                AvgWords = 0,
                UserId = user.Id,
                VisitStreak = 0,
                LastUpdateAt = DateTime.UtcNow
            };

            await _unitOfWork.UserStatistics.AddAsync(statistics);
            UserRole? role = await _unitOfWork.UserRoles.GetByNameAsync(context.Message.RoleName!);

            UserInfo userInfo = new UserInfo()
            {
                Id = context.Message.InfoId,
                Nickname = context.Message.Nickname,
                RoleId = role!.Id,
                Additional = "",
                IsRemoved = false,
                CreationalDate = DateTime.UtcNow,
                UserId = user.Id
            };

            await _unitOfWork.UserInfos.AddAsync(userInfo);

            _logger.LogInformation("[+] [Identity Create Consumer] Succesfully get message." +
                "{0}:{1}:{2} has been added", user.Id, user.Email, userInfo.Nickname);
        }
    }
}
