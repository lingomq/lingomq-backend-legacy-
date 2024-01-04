using Topics.Domain.Entities;

namespace Topics.DataAccess.Dapper.Contracts;
public interface ITopicStatisticsRepository : IGenericRepository<TopicStatistics>
{
    Task<List<TopicStatistics>> GetByTopicIdAsync(Guid id);
}
