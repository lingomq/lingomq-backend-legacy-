using Achievements.DataAccess.Dapper.Contracts;
using Achievements.Domain.Contracts;
using Achievements.Domain.Entities;
using Achievements.Domain.Exceptions.ClientExceptions;

namespace Achievements.Application.Services.UserAchievementsActions;
public class UserAchievementService : IUserAchievementService
{
    private readonly IUnitOfWork _unitOfWork;
    public UserAchievementService(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    public async Task<List<UserAchievement>> GetAsync(int count)
    {
        List<UserAchievement> userAchievements = await _unitOfWork.UserAchievements.GetAsync(count);
        return userAchievements;
    }

    public async Task<UserAchievement> GetAsync(Guid id)
    {
        UserAchievement? userAchievement = await _unitOfWork.UserAchievements.GetByIdAsync(id);
        if (userAchievement is null)
            throw new NotFoundException<UserAchievement>();

        return userAchievement;
    }

    public async Task<int> GetAchievementsCount(Guid id)
    {
        int count = await _unitOfWork.UserAchievements.GetCountAchievementsByUserIdAsync(id);
        return count;
    }

    public async Task<List<UserAchievement>> GetByUserId(Guid id)
    {
        List<UserAchievement> achievements = await _unitOfWork.UserAchievements.GetByUserIdAsync(id);
        return achievements;
    }
}
