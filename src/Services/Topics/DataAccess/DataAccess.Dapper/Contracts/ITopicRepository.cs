using Topics.Domain.Entities;
using Topics.Domain.Models;

namespace Topics.DataAccess.Dapper.Contracts;
public interface ITopicRepository : IGenericRepository<Topic>
{
    Task<List<Topic>> GetAsync(int start = 0, int stop = int.MaxValue);
    Task<List<Topic>> GetByTopicFiltersAsync(TopicFilters filters);
}
