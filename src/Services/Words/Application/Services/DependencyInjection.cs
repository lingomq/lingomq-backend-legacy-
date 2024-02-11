using Words.Application.Services.DataMigrator;
using Words.Application.Services.LanguageActions;
using Words.Application.Services.UserWordActions;
using Words.Application.Services.WordChecker;
using Words.Application.Services.WordTypeActions;
using Words.Domain.Contracts;

namespace Microsoft.Extensions.DependencyInjection;
public static class ApplicationsDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<IUserWordService, UserWordService>();
        services.AddScoped<IWordTypeService, WordTypeService>();
        services.AddScoped<IWordChecker, WordChecker>();
        services.AddScoped<IDataMigrator, DataMigrator>();

        return services;
    }
}
