using Topics.Domain.Entities;

namespace Topics.Domain.Contracts;
public interface ITopicStatisticsService
{
    Task<List<TopicStatistics>> GetAsync(int count);
    Task<List<TopicStatistics>> GetByTopicIdAsync(Guid id);
    Task<TopicStatistics> GetAsync(Guid id);
    Task CreateAsync(TopicStatistics topicStatistics);
    Task UpdateAsync(TopicStatistics topicStatistics);
    Task DeleteAsync(Guid id);
}
