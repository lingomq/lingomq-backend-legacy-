using Achievements.DataAccess.Dapper.Contracts;
using Achievements.Domain.Contracts;
using Achievements.Domain.Entities;

namespace Achievements.Application.Services.UserAchievementActions;
public class UserAchievementService : IUserAchievementService
{
    private readonly IUnitOfWork _unitOfWork;
    public UserAchievementService(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    public async Task<int> GetAchievementsCount(Guid userId)
    {
        int count = await _unitOfWork.UserAchievements.GetCountAchievementsByUserIdAsync(userId);
        return count;
    }

    public async Task<List<UserAchievement>> GetAsync(Guid userId) =>
        await _unitOfWork.UserAchievements.GetByUserIdAsync(userId);
}
