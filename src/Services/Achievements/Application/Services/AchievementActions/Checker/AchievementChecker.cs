using Achievements.Domain.Entities;
using EventBus.Entities.Achievements;

namespace Achievements.Application.Services.AchievementActions.Checker;
public class AchievementChecker
{
    public List<Achievement> CheckAndGetNewAchievements(List<Achievement> achievements, CheckAchievements checkAchievement)
    {
        IAchievementValidator achievementValidator = new WordChecker();
        List<Achievement> result = new List<Achievement>();
        result.AddRange(achievementValidator.GetAchievementIfExist(achievements, checkAchievement));

        return result;
    }
}
