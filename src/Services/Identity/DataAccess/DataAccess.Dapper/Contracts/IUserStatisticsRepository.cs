using Identity.Domain.Entities;

namespace DataAccess.Dapper.Contracts;
public interface IUserStatisticsRepository : IGenericRepository<UserStatistics>
{
    Task<UserStatistics?> GetByUserIdAsync(Guid id);
    Task<int> GetCountOfDaysByUserIdAsync(Guid id);
}
