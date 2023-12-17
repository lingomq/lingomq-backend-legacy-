using Identity.Domain.Entities;

namespace Identity.Domain.Contracts;
public interface IUserStatisticsService
{
    Task<UserStatistics> GetById(Guid id);
    Task AddHourToStatistics(Guid id);
    Task AddWordToStatistics(Guid id);
    Task AddVisitationToStatistics(Guid id);
}
