using Authentication.Application.Services.DataMigrator;
using Authentication.WebApi.Middlewares;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection.Application;
using System.Reflection;

namespace Authentication.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettins.json", true, true)
            .AddEnvironmentVariables();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwagger();
        builder.Services.AddPostgresDataAccess(builder.Configuration);
        builder.Services.AddApplicationMassTransit(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddFluentMigratorCore()
        .ConfigureRunner(cr => cr
        .AddPostgres()
        .WithGlobalConnectionString(builder.Configuration["ConnectionStrings:Dev:Authentication"])
        .ScanIn(Assembly.GetAssembly(typeof(DataAccess.Dapper.Postgres.Migrations.Initial))).For.Migrations());

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
