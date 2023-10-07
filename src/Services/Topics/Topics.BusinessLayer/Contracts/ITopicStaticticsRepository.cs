
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Contracts
{
    public interface ITopicStaticticsRepository : IGenericRepository<TopicStatistics>
    {
        Task<List<TopicStatistics>> GetByTopicIdAsync(Guid id);
    }
}
