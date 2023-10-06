using Achievements.DomainLayer.Entities;

namespace Achievements.BusinessLayer.Contracts
{
    public interface IUserAchievementRepository : IGenericRepository<UserAchievement>
    {
        Task<List<UserAchievement>> GetByUserIdAsync(Guid id);
        Task<int> GetCountAchievementsByUserIdAsync(Guid id);
    }
}
