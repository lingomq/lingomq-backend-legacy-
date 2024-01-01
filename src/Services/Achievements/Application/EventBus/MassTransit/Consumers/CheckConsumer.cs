using Achievements.Application.Services.AchievementActions.Checker;
using Achievements.DataAccess.Dapper.Contracts;
using Achievements.Domain.Entities;
using EventBus.Entities.Achievements;
using MassTransit;

namespace Achievements.BusinessLayer.MassTransit.Consumers
{
    public class CheckConsumer : IConsumer<CheckAchievements>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CheckConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<CheckAchievements> context)
        {
            await CheckAndAddUserAchievements(context.Message);
        }

        public async Task CheckAndAddUserAchievements(CheckAchievements checkAchievements)
        {
            AchievementChecker achievementChecker = new AchievementChecker();
            List<Achievement> achievements = new List<Achievement>();
            achievements.AddRange(achievementChecker
                .CheckAndGetNewAchievements(
                await _unitOfWork.Achievements.GetAsync(),
                checkAchievements));

            if (achievements.Any())
            {
                foreach (var achievement in achievements)
                {
                    UserAchievement userAchievement = new UserAchievement()
                    {
                        Id = Guid.NewGuid(),
                        UserId = checkAchievements.UserId,
                        AchievementId = achievement.Id,
                        DateOfReceipt = DateTime.UtcNow
                    };

                    await _unitOfWork.UserAchievements.CreateAsync(userAchievement);
                }
            }
        }
    }
}
