using Achievements.Domain.Entities;

namespace Achievements.Domain.Contracts;
public interface IUserAchievementService
{
    Task<List<UserAchievement>> GetAsync(Guid id);
    Task<int> GetAchievementsCount(Guid userId);
}
