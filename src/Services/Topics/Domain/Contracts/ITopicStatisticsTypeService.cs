using Topics.Domain.Entities;

namespace Topics.Domain.Contracts;
public interface ITopicStatisticsTypeService
{
    Task<List<TopicStatisticsType>> GetAsync(int count);
    Task CreateAsync(TopicStatisticsType topicStatisticsType);
    Task UpdateAsync(TopicStatisticsType topicStatisticsType);
    Task DeleteAsync(Guid id);
}
