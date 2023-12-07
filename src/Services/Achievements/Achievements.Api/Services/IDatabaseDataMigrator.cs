namespace Achievements.Api.Services
{
    public interface IDatabaseDataMigrator
    {
        Task AddAchievementsAsync();
        Task MigrateAsync();
    }
}
