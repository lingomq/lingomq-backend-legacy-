using Achievements.Domain.Entities;

namespace Achievements.DataAccess.Dapper.Contracts;
public interface IUserAchievementRepository
{
    Task<List<UserAchievement>> GetAsync(int range);
    Task<UserAchievement?> GetByIdAsync(Guid id);
    Task<List<UserAchievement>> GetByUserIdAsync(Guid id);
    Task<int> GetCountAchievementsByUserIdAsync(Guid id);
    Task CreateAsync(UserAchievement userAchievement);
}
