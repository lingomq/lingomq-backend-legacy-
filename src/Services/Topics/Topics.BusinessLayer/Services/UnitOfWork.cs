using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Services;

public class UnitOfWork : IUnitOfWork
{
    public ILanguageRepository Languages { get; }
    public ITopicLevelRepository TopicLevels { get; }
    public ITopicRepository Topics { get; }
    public ITopicStaticticsRepository TopicStatistics { get; }
    public ITopicStatisticsTypeRepository StatisticsTypes { get; }
    public IUserRepository Users { get; }

    public UnitOfWork(ILanguageRepository languageRepository,
        ITopicLevelRepository levelRepository,
        ITopicRepository topicRepository,
        ITopicStaticticsRepository staticticsRepository,
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