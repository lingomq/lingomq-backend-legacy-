using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.UnitTests.Common.Factories
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
