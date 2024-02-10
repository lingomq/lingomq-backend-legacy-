using Microsoft.Extensions.Configuration;
using Notifications.DataAccess.Dapper;
using Notifications.DataAccess.Dapper.Contracts;
using Notifications.DataAccess.Dapper.Postgres.Realizations;
using Npgsql;
using System.Data;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnection>
        (
            (serviceProvider) => new NpgsqlConnection(configuration["ConnectionStrings:Dev:Notification"])
        );
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
