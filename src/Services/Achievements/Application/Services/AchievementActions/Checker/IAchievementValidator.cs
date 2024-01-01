using Achievements.Domain.Entities;
using EventBus.Entities.Achievements;

namespace Achievements.Application.Services.AchievementActions.Checker;
public interface IAchievementValidator
{
    List<Achievement> GetAchievementIfExist(List<Achievement> achievements, CheckAchievements checkAchievements);
}
