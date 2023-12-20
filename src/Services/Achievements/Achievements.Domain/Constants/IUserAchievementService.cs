using Achievements.Domain.Entities;

namespace Achievements.Domain.Constants;
public interface IUserAchievementService
{
    Task<List<UserAchievement>> GetAsync(int count);
    Task<UserAchievement> GetAsync(Guid id);
    Task<int> GetAchievementsCount(Guid id);
    Task<UserAchievement> GetByUserId(Guid id);
}
