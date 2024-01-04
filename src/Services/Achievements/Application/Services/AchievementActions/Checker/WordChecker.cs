using Achievements.Domain.Entities;
using EventBus.Entities.Achievements;

namespace Achievements.Application.Services.AchievementActions.Checker;
public class WordChecker : IAchievementValidator
{
    public List<Achievement> GetAchievementIfExist(List<Achievement> achievements, CheckAchievements checkAchievements)
    {
        List<Achievement> gottenAchievements = new List<Achievement>();
        if (checkAchievements.WordsCount == 1)
        {
            gottenAchievements.Add(achievements.FirstOrDefault(x => x.Name == "Начало начал")!);
        }

        return gottenAchievements;
    }
}
