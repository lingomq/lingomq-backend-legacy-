using FluentMigrator.Runner;
using Identity.Api.Middlewares;
using Identity.Application.Services.DataMigrator;
using System.Reflection;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args); 
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwagger();
        builder.Services.AddPostgresDataAccess(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddJwtAuth(builder.Configuration);
        builder.Services.AddFluentMigratorCore()
        .ConfigureRunner(cr => cr
        .AddPostgres()
        .WithGlobalConnectionString(builder.Configuration["ConnectionStrings:Dev:Identity"])
        .ScanIn(Assembly.GetAssembly(typeof(Identity.DataAccess.Providers.Dapper.Migrations.InitialMigration))).For.Migrations());

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.UseMiddleware<ExceptionCatchingMiddleware>();

        using (var serviceScope = app.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            var runner = services.GetRequiredService<IMigrationRunner>();
            var dataMigrator = services.GetRequiredService<IDataMigrator>();
            runner.MigrateUp();

            dataMigrator.MigrateAsync().Wait();
        }

        app.Run();
    }
}
