using Topics.DataAccess.Dapper.Contracts;

namespace Topics.DataAccess.Dapper.Postgres;
public class UnitOfWork : IUnitOfWork
{
    public ILanguageRepository Languages { get; }
    public ITopicLevelRepository TopicLevels { get; }
    public ITopicRepository Topics { get; }
    public ITopicStatisticsRepository TopicStatistics { get; }
    public ITopicStatisticsTypeRepository StatisticsTypes { get; }
    public IUserRepository Users { get; }

    public UnitOfWork(ILanguageRepository languageRepository,
        ITopicLevelRepository levelRepository,
        ITopicRepository topicRepository,
        ITopicStatisticsRepository staticticsRepository,
        ITopicStatisticsTypeRepository statisticsTypeRepository,
        IUserRepository userRepository)
    {
        Languages = languageRepository;
        TopicLevels = levelRepository;
        Topics = topicRepository;
        TopicStatistics = staticticsRepository;
        StatisticsTypes = statisticsTypeRepository;
        Users = userRepository;
    }
}
