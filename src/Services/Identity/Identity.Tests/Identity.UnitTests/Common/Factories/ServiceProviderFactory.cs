using FluentMigrator.Runner;
using Identity.BusinessLayer.MassTransit;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;
using System.Reflection;

namespace Identity.UnitTests.Common.Factories
{
    public class ServiceProviderFactory
    {
        public static ServiceProvider Create(IConfiguration configuration)
        {
            return new ServiceCollection()
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddTransient<IDbConnection>((sp) => new NpgsqlConnection(configuration["ConnectionStrings:Test"]))
                .AddMassTransit(x => x.UsingInMemory())
                .AddFluentMigratorCore()
                .ConfigureRunner(cr => cr
                .AddPostgres()
                .WithGlobalConnectionString(configuration["ConnectionStrings:Test"])
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                .AddSingleton<PublisherBase>().BuildServiceProvider(true);
        }
    }
}
