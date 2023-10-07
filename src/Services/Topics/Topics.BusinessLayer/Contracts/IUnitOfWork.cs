namespace Topics.BusinessLayer.Contracts
{
    public interface IUnitOfWork
    {
        ILanguageRepository Languages { get; }
        ITopicLevelRepository TopicLevels { get; }
        ITopicRepository Topics { get; }
        ITopicStatisticsRepository TopicStatistics { get; }
        ITopicStatisticsTypeRepository StatisticsTypes { get; }
        IUserRepository Users { get; }
    }
}
