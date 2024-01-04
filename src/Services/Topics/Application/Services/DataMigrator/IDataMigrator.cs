namespace Topics.Application.Services.DataMigrator;

public interface IDataMigrator
{
    Task AddLanguagesAsync();
    Task AddTopicStatisticsTypeAsync();
    Task AddTopicLevelAsync();
    Task MigrateAsync();
}
