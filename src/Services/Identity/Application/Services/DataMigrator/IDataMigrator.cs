namespace Identity.Application.Services.DataMigrator;
public interface IDataMigrator
{
    Task AddRolesAsync();
    Task MigrateAsync();
}
