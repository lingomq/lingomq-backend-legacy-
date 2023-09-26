using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.UnitTest.Common.Factories
{
    public class Migrator
    {
        public static void Migrate(ServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}
