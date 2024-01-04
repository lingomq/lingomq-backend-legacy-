using Achievements.Domain.Entities;

namespace Achievements.Domain.Contracts;
public interface IAchievementService
{
    Task<List<Achievement>> GetAsync(int count);
    Task<Achievement> GetAsync(Guid id);
    Task CreateAsync(Achievement achievement);
    Task UpdateAsync(Achievement achievement);
    Task DeleteAsync(Guid id);
}
