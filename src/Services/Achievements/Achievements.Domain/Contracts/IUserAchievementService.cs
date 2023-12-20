using Achievements.Domain.Entities;

namespace Achievements.Domain.Contracts;
public interface IUserAchievementService
{
    Task<List<UserAchievement>> GetAsync(int count);
    Task<UserAchievement> GetAsync(Guid id);
    Task<int> GetAchievementsCount(Guid id);
    Task<List<UserAchievement>> GetByUserId(Guid id);
}
