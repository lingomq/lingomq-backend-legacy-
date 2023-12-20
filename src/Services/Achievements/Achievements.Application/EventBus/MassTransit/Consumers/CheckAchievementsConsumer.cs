using Achievements.DataAccess.Dapper.Contracts;
using Achievements.Domain.Entities;
using EventBus.Entities.Achievements;
using MassTransit;

namespace Achievements.Application.EventBus.MassTransit.Consumers;
public class CheckAchievementsConsumer : IConsumer<CheckAchievements>
{
    private readonly IUnitOfWork _unitOfWork;
    public CheckAchievementsConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<CheckAchievements> context)
    {
        await CheckAndAddUserAchievements(context.Message);
    }

    public async Task CheckAndAddUserAchievements(CheckAchievements checkAchievements)
    {
        List<Achievement> achievements = new List<Achievement>();
        achievements.AddRange(await CheckWords(checkAchievements.WordsCount));
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

    public async Task<List<Achievement>> CheckWords(int count)
    {
        List<Achievement> achievements = await _unitOfWork.Achievements.GetAsync(10);
        List<Achievement> gottenAchievements = new List<Achievement>();
        if (count == 1)
        {
            gottenAchievements.Add(achievements.FirstOrDefault(x => x.Name == "Начало начал")!);
        }

        return gottenAchievements;
    }
}