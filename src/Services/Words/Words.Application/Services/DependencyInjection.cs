using Words.Application.Services.DataMigrator;
using Words.Application.Services.LanguageActions;
using Words.Application.Services.UserWordActions;
using Words.Application.Services.UserWordTypeActions;
using Words.Application.Services.WordChecker;
using Words.Application.Services.WordTypeActions;
using Words.Domain.Contracts;

namespace Microsoft.Extensions.DependencyInjection.Applications;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<IUserWordService, UserWordService>();
        services.AddScoped<IUserWordTypeService, UserWordTypeService>();
        services.AddScoped<IWordTypeService, WordTypeService>();
        services.AddScoped<IWordChecker, WordChecker>();
        services.AddTransient<IDataMigrator, DataMigrator>();

        return services;
    }
}
