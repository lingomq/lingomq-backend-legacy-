using Achievements.DataAccess.Dapper.Contracts;
using Achievements.DataAccess.Dapper.Postgres;
using Achievements.DataAccess.Dapper.Postgres.Realizations;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyInjection
{
    public static IServiceCollection AddPostgresDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnection>
        (
            (serviceProvider) => new NpgsqlConnection(configuration["ConnectionStrings:Dev:Achievements"])
        );

        services.AddScoped<IAchievementRepository, AchievementRepository>();
        services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
