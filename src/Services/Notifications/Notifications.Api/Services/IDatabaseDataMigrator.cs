namespace Notifications.Api.Services
{
    public interface IDatabaseDataMigrator
    {
        Task AddNotificationTypes();
        Task Migrate();
    }
}
