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
            (serviceProvider) => new NpgsqlConnection(configuration["ConnectionStrings:Dev:Words"])
        );

        services.AddScoped<IUserWordTypeRepository, UserWordTypeRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IUserWordRepository, UserWordRepository>();
        services.AddScoped<IWordTypeRepository, WordTypeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

