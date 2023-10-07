using System.Data;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Services.Repositories
{
    public class TopicLevelRepository : GenericRepository<TopicLevel>, ITopicLevelRepository
    {
        private readonly static string Get =
            "SELECT id as \"Id\", " +
            "name as \"LevelName\" " +
            "FROM topic_levels ";
        private readonly static string GetById = Get +
            "WHERE id = @Id";
        private readonly static string GetRange = Get +
            "LIMIT @Count;";
        private readonly static string Create =
            "INSERT INTO topic_levels " +
            "(id, name) " +
            "VALUES (@Id, @LevelName)";
        private readonly static string Update =
            "UPDATE topic_levels SET " +
            "name = @LevelName " +
            "WHERE id = @Id";
        private readonly static string Delete =
            "DELETE FROM topic_levels " +
            "WHERE id = @Id";

        private readonly IDbConnection _connection;
        public TopicLevelRepository(IDbConnection connection) : base(connection) =>
            _connection = connection;

        public async Task AddAsync(TopicLevel entity)
        {
            await ExecuteByTemplateAsync(Create, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await ExecuteByTemplateAsync(Delete, new { Id = id});
        }

        public async Task<List<TopicLevel>> GetAsync(int range)
        {
            return await GetByQueryAsync(GetRange, new { Count = range });
        }

        public async Task<TopicLevel?> GetByIdAsync(Guid id)
        {
            IEnumerable<TopicLevel> levels = await GetByQueryAsync(GetById, new { Id = id });
            return levels.FirstOrDefault();
        }

        public async Task UpdateAsync(TopicLevel entity)
        {
            await ExecuteByTemplateAsync(Update, entity);
        }
    }
}
