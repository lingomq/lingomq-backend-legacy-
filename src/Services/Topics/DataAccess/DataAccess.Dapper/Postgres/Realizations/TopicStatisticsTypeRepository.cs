using System.Data;
using Topics.DataAccess.Dapper.Contracts;
using Topics.DataAccess.Dapper.Postgres.RawQueries;
using Topics.DataAccess.Dapper.Utils;
using Topics.Domain.Contracts;
using Topics.Domain.Entities;

namespace Topics.DataAccess.Dapper.Postgres.Realizations;
public class TopicStatisticsTypeRepository : GenericRepository<TopicStatisticsType>, ITopicStatisticsTypeRepository
{
    public TopicStatisticsTypeRepository(IDbConnection connection) : base(connection)
    {
    }

    public async Task AddAsync(TopicStatisticsType entity)=>
        await ExecuteByTemplateAsync(TopicStatisticsTypeQueries.Create, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(TopicStatisticsTypeQueries.Delete, new { Id = id });

    public async Task<List<TopicStatisticsType>> GetAsync(int count) =>
        await QueryListAsync(TopicStatisticsTypeQueries.GetRange, new { Count = count });

    public async Task<TopicStatisticsType?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(TopicStatisticsTypeQueries.GetById, new { Id = id });

    public async Task UpdateAsync(TopicStatisticsType entity) =>
        await ExecuteByTemplateAsync(TopicStatisticsTypeQueries.Update, entity);
}
