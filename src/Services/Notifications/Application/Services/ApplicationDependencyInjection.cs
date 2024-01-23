using Notifications.Application.Services.DataMigrator;
using Notifications.Application.Services.NotificationActions;
using Notifications.Application.Services.UserNotificationActions;
using Notifications.Domain.Contracts;

namespace Microsoft.Extensions.DependencyInjection;
public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddScoped<IUserNotificationService, UserNotificationService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddTransient<IDataMigrator, DataMigrator>();

        return services;
    }
}
