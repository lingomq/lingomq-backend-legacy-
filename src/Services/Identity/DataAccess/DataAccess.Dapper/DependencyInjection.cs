using DataAccess.Dapper;
using DataAccess.Dapper.Contracts;
using DataAccess.Dapper.Postgres.Realizations;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnection>
        (
            (serviceProvider) => new NpgsqlConnection(configuration["ConnectionStrings:Dev:Identity"])
        );
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserInfoRepository, UserInfoRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IUserStatisticsRepository, UserStatisticsRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
