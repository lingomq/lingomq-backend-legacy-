namespace Authentication.Application.Services.DataMigrator;
public interface IDataMigrator
{
    Task AddRoles();
    Task MigrateAsync();
}
