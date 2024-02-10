using System.Data;
using Topics.DataAccess.Dapper.Contracts;
using Topics.DataAccess.Dapper.Postgres.RawQueries;
using Topics.DataAccess.Dapper.Utils;
using Topics.Domain.Entities;

namespace Topics.DataAccess.Dapper.Postgres.Realizations;
public class TopicLevelRepository : GenericRepository<TopicLevel>, ITopicLevelRepository
{
    public TopicLevelRepository(IDbConnection connection) : base(connection)
    {
    }

    public async Task AddAsync(TopicLevel entity) =>
        await ExecuteByTemplateAsync(TopicLevelQueries.Create, entity);
    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(TopicLevelQueries.Delete, new { Id = id });

    public async Task<List<TopicLevel>> GetAsync(int range = int.MaxValue) =>
        await QueryListAsync(TopicLevelQueries.GetRange, new { Count = range });

    public async Task<TopicLevel?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(TopicLevelQueries.GetById, new { Id = id });

    public async Task UpdateAsync(TopicLevel entity) =>
        await ExecuteByTemplateAsync(TopicLevelQueries.Update, entity);
}
