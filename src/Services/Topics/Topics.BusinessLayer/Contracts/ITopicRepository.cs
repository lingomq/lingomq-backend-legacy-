using Topics.BusinessLayer.Models;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Contracts
{
    public interface ITopicRepository : IGenericRepository<Topic>
    {
        Task<List<Topic>> GetAsync(int start = 0, int stop = int.MaxValue);
        Task<List<Topic>> GetByTopicFiltersAsync(TopicFilters filters);

    }
}
