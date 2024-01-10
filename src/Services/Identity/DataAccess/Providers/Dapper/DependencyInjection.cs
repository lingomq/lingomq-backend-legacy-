using Identity.DataAccess.Contracts;
using Identity.DataAccess.Providers.Dapper.Realizations;
using Identity.DataAccess.Providers.Dapper;
using Npgsql;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyInjection
{
    public static IServiceCollection AddPostgresDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnection>
        (
            (serviceProvider) => new NpgsqlConnection(configuration["ConnectionStrings:Dev:Identity"])
        );

        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserCredentialsRepository, UserCredentialsRepository>();
        services.AddScoped<IUserInfoRepository, UserInfoRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserSensitiveDataRepository, UserSensitiveDataRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
