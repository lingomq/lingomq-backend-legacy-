using Topics.Domain.Entities;

namespace Topics.Domain.Contracts;
public interface ITopicLevelService
{
    Task<List<TopicLevel>> GetAsync(int count);
    Task<TopicLevel> GetAsync(Guid id);
    Task CreateAsync(TopicLevel topicLevel);
    Task UpdateAsync(TopicLevel topicLevel);
    Task DeleteAsync(Guid id);
}
