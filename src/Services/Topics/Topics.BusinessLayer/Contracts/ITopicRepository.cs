using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Contracts
{
    public interface ITopicRepository : IGenericRepository<Topic>
    {
        Task<List<Topic>> GetByLanguageId(Guid id);
        Task<List<Topic>> GetByDateRange(DateTime start, DateTime stop);
        Task<List<Topic>> GetByTopicLevelId(Guid id);
    }
}
