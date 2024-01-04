using Topics.Application.Services.DataMigrator;
using Topics.Application.Services.TopicActions;
using Topics.Application.Services.TopicLevelActions;
using Topics.Application.Services.TopicStatisticsActions;
using Topics.Application.Services.TopicStatisticsTypeActions;
using Topics.Domain.Contracts;

namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITopicLevelService, TopicLevelService>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<ITopicStatisticsService, TopicStatisticsService>();
        services.AddScoped<ITopicStatisticsTypeService, TopicStatisticsTypeService>();
        services.AddScoped<IDataMigrator, DataMigrator>();

        return services;
    }
}
