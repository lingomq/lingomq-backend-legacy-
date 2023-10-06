using Achievements.DomainLayer.Entities;

namespace Achievements.BusinessLayer.Contracts
{
    public interface IUserAchievementRepository : IGenericRepository<UserAchievement>
    {
        Task<List<UserAchievement>?> GetByUserId(Guid id);
        Task<int> GetCountAchievementsByUserId(Guid id);
    }
}
