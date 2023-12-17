using Identity.Domain.Entities;

namespace Identity.Domain.Contracts;
public interface IUserStatisticsService
{
    Task<UserStatistics> GetByIdAsync(Guid id);
    Task AddHourToStatisticsAsync(Guid id);
    Task AddWordToStatisticsAsync(Guid id);
    Task AddVisitationToStatisticsAsync(Guid id);
}
