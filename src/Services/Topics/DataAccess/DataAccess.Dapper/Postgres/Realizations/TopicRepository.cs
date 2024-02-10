using Dapper;
using System.Data;
using Topics.DataAccess.Dapper.Contracts;
using Topics.DataAccess.Dapper.Postgres.RawQueries;
using Topics.DataAccess.Dapper.Utils;
using Topics.Domain.Entities;
using Topics.Domain.Models;

namespace Topics.DataAccess.Dapper.Postgres.Realizations;
public class TopicRepository : GenericRepository<Topic>, ITopicRepository
{
    private readonly IDbConnection _connection;
    public TopicRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;

    public async Task AddAsync(Topic entity) =>
        await ExecuteByTemplateAsync(TopicQueries.Create, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(TopicQueries.Delete, new { Id = id });

    public async Task<List<Topic>> GetAsync(int start = 0, int stop = int.MaxValue) =>
        await QueryListAsync(TopicQueries.GetRange + TopicQueries.PaginationAndOrderByDate, new { Skip = start, Take = stop });

    public async Task<List<Topic>> GetAsync(int range = int.MaxValue) =>
        await QueryListAsync(TopicQueries.GetRange, new { Count = range });

    public async Task<Topic?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(TopicQueries.GetById, new { Id = id });

    public async Task<List<Topic>> GetByTopicFiltersAsync(TopicFilters filters)
    {
        string sql = TopicQueries.GetByDateRange;
        filters.Search = "%" + filters.Search + "%";

        sql += filters.LanguageId != null ? TopicQueries.GetByLanguageId : "";
        sql += filters.LevelId != null ? TopicQueries.GetByTopicLevelId : "";
        sql += TopicQueries.GetBySearch;
        sql += TopicQueries.PaginationAndOrderByDate;

        filters.Count = 20;

        return await QueryListAsync(sql, filters);
    }

    public async Task UpdateAsync(Topic entity) =>
        await ExecuteByTemplateAsync(TopicQueries.Update, entity);
    protected override async Task<List<Topic>> QueryListAsync<TE>(string sql, TE entity) where TE : class
    {
        IEnumerable<Topic> topics;
        topics = await _connection.QueryAsync<Topic, Language, TopicLevel, Topic>(
            sql, (topic, language, level) =>
            {
                topic.Language = language;
                topic.LanguageId = language.Id;

                topic.TopicLevel = level;
                topic.TopicLevelId = level.Id;
                return topic;
            }, entity, splitOn: "id");

        return !topics.Any() ? new List<Topic>() : topics.ToList();
    }
}
