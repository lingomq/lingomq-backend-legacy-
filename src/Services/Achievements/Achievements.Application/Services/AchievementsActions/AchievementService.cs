using Achievements.DataAccess.Dapper.Contracts;
using Achievements.Domain.Contracts;
using Achievements.Domain.Entities;
using Achievements.Domain.Exceptions.ClientExceptions;

namespace Achievements.Application.Services.AchievementsActions;
public class AchievementService : IAchievementService
{
    private readonly IUnitOfWork _unitOfWork;
    public AchievementService(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    public async Task<List<Achievement>> GetAsync(int count)
    {
        List<Achievement> achievements = await _unitOfWork.Achievements.GetAsync(count);
        return achievements;
    }

    public async Task<Achievement> GetAsync(Guid id)
    {
        Achievement? achievement = await _unitOfWork.Achievements.GetByIdAsync(id);
        if (achievement is null)
            throw new NotFoundException<Achievement>();

        return achievement;
    }

    public async Task CreateAsync(Achievement achievement)
    {
        await _unitOfWork.Achievements.AddAsync(achievement);
    }

    public async Task UpdateAsync(Achievement achievement)
    {
        if (await _unitOfWork.Achievements.GetByIdAsync(achievement.Id) is null)
            throw new InvalidDataException<Achievement>("Data hasn't been found");

        await _unitOfWork.Achievements.UpdateAsync(achievement);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (await _unitOfWork.Achievements.GetByIdAsync(id) is null)
            throw new InvalidDataException<Achievement>("Data hasn't been found");

        await _unitOfWork.Achievements.DeleteAsync(id);
    }
}
