using Identity.DomainLayer.Entities;

namespace Identity.BusinessLayer.Contracts
{
    public interface IUserStatisticsRepository : IGenericRepository<UserStatistics, UserStatistics>
    {
        Task<UserStatistics?> GetByUserIdAsync(Guid id);
        Task<int> GetCountOfDaysByUserIdAsync(Guid id);
    }
}
