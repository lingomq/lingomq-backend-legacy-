using System.Data;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Services.Repositories
{
    public class TopicStatisticsTypeRepository : GenericRepository<TopicStatisticsType>, ITopicStatisticsTypeRepository
    {
        private readonly static string Get =
            "SELECT id as \"Id\", " +
            "name as \"TypeName\" " +
            "FROM topic_statistics_types ";
        private readonly static string GetRange = Get +
            "LIMIT @Count";
        private readonly static string GetById = Get +
            "WHERE id = @Id";
        private readonly static string Create =
            "INSERT INTO topic_statistics_types " +
            "(id, name) " +
            "VALUES (@Id, @TypeName)";
        private readonly static string Update =
            "UPDATE topic_statistics_types SET" +
            "name = @TypeName " +
            "WHERE id = @Id";
        private readonly static string Delete =
            "DELETE FROM topic_statistics_types " +
            "WHERE id = @Id";

        private readonly IDbConnection _connection;
        public TopicStatisticsTypeRepository(IDbConnection connection) : base(connection) =>
            _connection = connection;

        public async Task AddAsync(TopicStatisticsType entity)
        {
            await ExecuteByTemplateAsync(Create, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await ExecuteByTemplateAsync(Delete, new { Id = id });
        }

        public async Task<List<TopicStatisticsType>> GetAsync(int range)
        {
            return await GetByQueryAsync(GetRange, new { Count = range });
        }

        public async Task<TopicStatisticsType?> GetByIdAsync(Guid id)
        {
            IEnumerable<TopicStatisticsType> types = await GetByQueryAsync(GetById, new { Id = id });
            return types.FirstOrDefault();
        }

        public async Task UpdateAsync(TopicStatisticsType entity)
        {
            await ExecuteByTemplateAsync(Update, entity);
        }
    }
}
