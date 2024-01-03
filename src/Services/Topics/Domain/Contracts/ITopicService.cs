using Topics.Domain.Entities;
using Topics.Domain.Models;

namespace Topics.Domain.Contracts;
public interface ITopicService
{
    Task<List<Topic>> GetAsync(int skip, int take);
    Task<Topic> GetAsync(Guid id);
    Task CreateAsync(Topic topic);
    Task UpdateAsync(Topic topic);
    Task DeleteAsync(Guid id);
    Task<List<Topic>> UseFilters(TopicFilters topicFilters);
}
