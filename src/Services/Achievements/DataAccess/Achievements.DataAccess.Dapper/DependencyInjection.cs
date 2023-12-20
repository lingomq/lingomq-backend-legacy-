using System.Data;
using Achievements.DataAccess.Dapper;
using Achievements.DataAccess.Dapper.Contracts;
using Achievements.DataAccess.Dapper.Postgres.Realizations;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnection>
        (
            (serviceProvider) => new NpgsqlConnection(configuration["ConnectionStrings:Dev:Achievements"])
        );
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();
        services.AddScoped<IAchievementRepository, AchievementRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
