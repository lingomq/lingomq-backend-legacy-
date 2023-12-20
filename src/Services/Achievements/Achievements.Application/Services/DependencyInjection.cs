using Achievements.Application.Services.AchievementsActions;
using Achievements.Application.Services.DataMigrator;
using Achievements.Domain.Constants;

namespace Microsoft.Extensions.DependencyInjection.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAchievementService, AchievementService>();
        services.AddScoped<IUserAchievementService, IUserAchievementService>();
        services.AddScoped<IDataMigrator, DataMigrator>();

        return services;
    }
}
