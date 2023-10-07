using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Contracts
{
    public interface ITopicRepository : IGenericRepository<Topic>
    {
        Task<List<Topic>> GetByLanguageIdAsync(Guid id);
        Task<List<Topic>> GetByDateRangeAsync(DateTime start, DateTime stop);
        Task<List<Topic>> GetByTopicLevelIdAsync(Guid id);
    }
}
