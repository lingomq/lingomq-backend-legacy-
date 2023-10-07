using System.Data;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Services.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly IDbConnection _connection;
        public TopicRepository(IDbConnection connection) =>
            _connection = connection;
        public Task AddAsync(Topic entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Topic>> GetAsync(int range)
        {
            throw new NotImplementedException();
        }

        public Task<List<Topic>> GetByDateRange(DateTime start, DateTime stop)
        {
            throw new NotImplementedException();
        }

        public Task<Topic?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Topic>> GetByLanguageId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Topic>> GetByTopicLevelId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Topic entity)
        {
            throw new NotImplementedException();
        }
    }
}
