using Achievements.Application.Services.AchievementActions;
using Achievements.Application.Services.DataMigrator;
using Achievements.Application.Services.UserAchievementActions;
using Achievements.Domain.Contracts;

namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAchievementService, AchievementService>();
        services.AddScoped<IUserAchievementService, UserAchievementService>();
        services.AddScoped<IDataMigrator, DataMigrator>();

        return services;
    }
}
