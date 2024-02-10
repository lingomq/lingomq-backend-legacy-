using Dapper;
using System.Data;
using Topics.DataAccess.Dapper.Contracts;
using Topics.DataAccess.Dapper.Postgres.RawQueries;
using Topics.DataAccess.Dapper.Utils;
using Topics.Domain.Entities;

namespace Topics.DataAccess.Dapper.Postgres.Realizations;
public class TopicStatisticsRepository : GenericRepository<TopicStatistics>, ITopicStatisticsRepository
{
    private readonly IDbConnection _connection;
    public TopicStatisticsRepository(IDbConnection connection) : base(connection)
    {
        _connection = connection;
    }

    public async Task AddAsync(TopicStatistics entity) =>
        await ExecuteByTemplateAsync(TopicStatisticsQueries.Create, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(TopicStatisticsQueries.Delete, new { Id = id });

    public async Task<List<TopicStatistics>> GetAsync(int range = int.MaxValue) =>
        await QueryListAsync(TopicStatisticsQueries.GetRange, new { Count = range });

    public async Task<TopicStatistics?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(TopicStatisticsQueries.GetById, new { Id = id });

    public async Task<List<TopicStatistics>> GetByTopicIdAsync(Guid id) =>
        await QueryListAsync(TopicStatisticsQueries.GetByTopicId, new { Id = id });

    public async Task UpdateAsync(TopicStatistics entity) =>
        await ExecuteByTemplateAsync(TopicStatisticsQueries.Update, entity);
    protected override async Task<List<TopicStatistics>> QueryListAsync<TE>(string sql, TE entity) where TE : class
    {
        IEnumerable<TopicStatistics> statistics;
        statistics = await _connection.QueryAsync<TopicStatistics, Topic, Language,
            TopicLevel, User, TopicStatisticsType, TopicStatistics>(
            sql, (statistic, topic, language, level, user, statisticsType) =>
            {
                statistic.Topic = topic;
                statistic.TopicId = topic.Id;
                statistic.User = user;
                statistic.UserId = user.Id;
                statistic.Type = statisticsType;
                statistic.StatisticsTypeId = statisticsType.Id;

                topic.TopicLevel = level;
                topic.TopicLevelId = level.Id;
                topic.Language = language;
                topic.LanguageId = language.Id;

                return statistic;
            }, entity, splitOn: "id");

        return !statistics.Any() ? new List<TopicStatistics>() : statistics.ToList();
    }
}
