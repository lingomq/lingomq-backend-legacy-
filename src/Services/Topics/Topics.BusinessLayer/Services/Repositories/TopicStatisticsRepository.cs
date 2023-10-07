using System.Data;
using Dapper;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Services.Repositories;

public class TopicStatisticsRepository : GenericRepository<TopicStatistics>, ITopicStaticticsRepository
{
    private static readonly string Get =
        "SELECT topic_statistics.id, " +
        "statistics_date as \"StatisticsDate\", " +
        "topics.id, " +
        "topics.title as \"Title\", " +
        "topics.content as \"Content\", " +
        "topics.icon as \"Icon\"," +
        "topics.creational_date as \"CreationalDate\", " +
        "languages.id, " +
        "languages.name as \"Name\", " +
        "topic_levels.id, " +
        "topic_levels.name as \"LevelName\"," +
        "users.id, " +
        "users.email as \"Email\", " +
        "users.phone as \"Phone\", " +
        "topic_statistics_types.id," +
        "topic_statistics_types.name as \"TypeName\" " +
        "FROM topic_statistics " +
        "JOIN topics ON topics.id = topic_statistics.topic_id " +
        "JOIN languages ON languages.id = topics.language_id" +
        "JOIN topic_levels ON topic_levels.id = topics.topic_level_id " +
        "JOIN users ON users.id = topic_statistics.user_id " +
        "JOIN topic_statistics_types ON topic_statistics_types.id = topic_statistics.topic_statistics_type_id ";
    private static readonly string GetRange = Get +
        "LIMIT @Count";
    private static readonly string GetById = Get +
        "WHERE topic_statistics.id = @Id";
    private static readonly string GetByTopicId = Get +
        "WHERE topics.id = @Id";
    private static readonly string Create =
        "INSERT INTO topic_statistics " +
        "(id, topic_id, user_id, topic_statistics_type_id, statistics_date) " +
        "VALUES " +
        "(@Id, @TopicId, @UserId, @StatisticsTypeId, @StatisticsDate)";
    private static readonly string Update =
        "UPDATE topic_statistics SET " +
        "topic_id = @TopicId, " +
        "user_id = @UserId, " +
        "statistics_type_id = @StatisticsTypeId," +
        "statistics_date = @StatisticsDate " +
        "WHERE id = @Id";
    private static readonly string Delete =
        "DELETE FROM topic_statistics " +
        "WHERE id = @Id";
    private readonly IDbConnection _connection;
    public TopicStatisticsRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;
    
    public async Task<TopicStatistics?> GetByIdAsync(Guid id)
    {
        List<TopicStatistics> statistics = await GetByQueryAsync(GetById, new { Id = id });
        return statistics.FirstOrDefault();
    }

    public async Task<List<TopicStatistics>> GetAsync(int range)
    {
        return await GetByQueryAsync(GetRange, new { Count = range });
    }

    public async Task AddAsync(TopicStatistics entity)
    {
        await ExecuteByTemplateAsync(Create, entity);
    }

    public async Task UpdateAsync(TopicStatistics entity)
    {
        await ExecuteByTemplateAsync(Update, entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await ExecuteByTemplateAsync(Delete, new { Id = id });
    }

    public async Task<List<TopicStatistics>> GetByTopicIdAsync(Guid id)
    {
        return await GetByQueryAsync(GetByTopicId, new { Id = id });
    }

    protected override async Task<List<TopicStatistics>> GetByQueryAsync<TE>(string sql, TE entity) where TE : class
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