namespace Achievements.Application.Services.DataMigrator
{
    public interface IDataMigrator
    {
        Task AddAchievementsAsync();
        Task MigrateAsync();
    }
}
