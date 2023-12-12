namespace Topics.Api.Services
{
    public interface IDatabaseDataMigrator
    {
        Task AddLanguagesAsync();
        Task AddTopicStatisticsTypeAsync();
        Task AddTopicLevelAsync();
        Task MigrateAsync();
    }
}
