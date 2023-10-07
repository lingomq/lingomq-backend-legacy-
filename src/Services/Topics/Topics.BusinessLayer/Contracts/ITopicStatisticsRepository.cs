
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Contracts
{
    public interface ITopicStatisticsRepository : IGenericRepository<TopicStatistics>
    {
        Task<List<TopicStatistics>> GetByTopicIdAsync(Guid id);
    }
}
