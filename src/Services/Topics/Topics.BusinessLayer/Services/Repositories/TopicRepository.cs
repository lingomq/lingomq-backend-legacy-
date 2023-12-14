using System;
using System.Data;
using Dapper;
using Topics.BusinessLayer.Contracts;
using Topics.BusinessLayer.Models;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Services.Repositories
{
    public class TopicRepository : GenericRepository<Topic>, ITopicRepository
    {
        private static readonly string Get =
            "SELECT topics.id, " +
            "title as \"Title\", " +
            "content as \"Content\", " +
            "icon as \"Icon\"," +
            "creational_date as \"CreationalDate\", " +
            "languages.id, " +
            "languages.name as \"Name\", " +
            "topic_levels.id, " +
            "topic_levels.name as \"LevelName\" " +
            "FROM topics " +
            "JOIN languages ON languages.id = topics.language_id " +
            "JOIN topic_levels ON topic_levels.id = topics.topic_level_id ";
        private static readonly string GetRange = Get + "LIMIT @Count ";
        private static readonly string PaginationAndOrderByDate = "ORDER BY creational_date OFFSET (@Skip) ROWS " +
            "FETCH NEXT (@Take) ROWS ONLY";
        private static readonly string GetById = Get + "WHERE topics.id = @Id";
        private static readonly string GetByDateRange = Get +
            "WHERE creational_date > @StartDate " +
            "AND creational_date < @FinishDate ";
        private static readonly string GetByLanguageId = Get + "AND languages.id = @LanguageId ";
        private static readonly string GetByTopicLevelId = Get + "AND topic_levels.id = @LevelId ";
        private static readonly string Create =
            "INSERT INTO topics " +
            "(id, title, content, icon, creational_date, language_id, topic_level_id) " +
            "VALUES " +
            "(@Id, @Title, @Content, @Icon, @CreationalDate, @LanguageId, @TopicLevelId)";
        private static readonly string Update =
            "UPDATE topics SET" +
            "title = @Title, " +
            "content = @Content, " +
            "icon = @Icon, " +
            "creational_date = @CreationalDate, " +
            "language_id = @LanguageId, " +
            "topic_level_id = @TopicLevelId " +
            "WHERE id = @Id";
        private static readonly string Delete =
            "DELETE FROM topics " +
            "WHERE id = @Id";
        private static readonly string Limit = "LIMIT @Count";
        private readonly IDbConnection _connection;
        public TopicRepository(IDbConnection connection) : base(connection) =>
            _connection = connection;
        public async Task AddAsync(Topic entity)
        {
            await ExecuteByTemplateAsync(Create, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await ExecuteByTemplateAsync(Delete, new { Id = id });
        }

        public async Task<List<Topic>> GetAsync(int range)
        {
            return await GetByQueryAsync(GetRange, new { Count = range });
        }

        public async Task<List<Topic>> GetAsync(int count, int start = 0, int stop = int.MaxValue)
        {
            return await GetByQueryAsync(GetRange + PaginationAndOrderByDate + Limit, new { Count = count, Skip = start, Take = stop });
        }

        public async Task<List<Topic>> GetByDateRangeAsync(DateTime start, DateTime stop, int startPagination = 0, int stopPagination = int.MaxValue)
        {
            return await GetByQueryAsync(GetByDateRange, new 
            {
                 Start = start,
                 Finish = stop
            });
        }

        public async Task<Topic?> GetByIdAsync(Guid id)
        {
            List<Topic> topics = await GetByQueryAsync(GetById, new { Id = id });
            return topics.FirstOrDefault();
        }

        public async Task<List<Topic>> GetByLanguageIdAsync(Guid id, int start = 0, int stop = int.MaxValue)
        {
            return await GetByQueryAsync(GetByLanguageId, new { Id = id });
        }

        public async Task<List<Topic>> GetByTopicFiltersAsync(TopicFilters filters)
        {
            string sql = GetByDateRange +
                filters.LanguageId != null ? GetByLanguageId : "" +
                filters.LevelId != null ? GetByTopicLevelId : "" +
                PaginationAndOrderByDate +
                Limit;

            filters.Count = 20;

            return await GetByQueryAsync(sql, filters);
                
        }

        public async Task<List<Topic>> GetByTopicLevelIdAsync(Guid id, int start = 0, int stop = int.MaxValue)
        {
            return await GetByQueryAsync(GetByTopicLevelId, new { Id = id });
        }

        public async Task UpdateAsync(Topic entity)
        {
            await ExecuteByTemplateAsync(Update, entity);
        }

        protected override async Task<List<Topic>> GetByQueryAsync<TE>(string sql, TE entity) where TE: class
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
}
