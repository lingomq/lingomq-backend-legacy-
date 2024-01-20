namespace Notifications.Application.Services.DataMigrator;
public interface IDataMigrator
{
    Task AddNotificationTypes();
    Task MigrateAsync();
}
