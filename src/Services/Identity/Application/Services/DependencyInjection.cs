using Identity.Domain.Contracts;
using Identity.Application.Services.Authentication;
using Identity.Application.Services.Confirmation;
using Identity.Application.Services.UserActions;
using Identity.Application.Services.Jwt;
using Identity.Application.Services.DataMigrator;

namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IConfirmationService, ConfirmationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDataMigrator, DataMigrator>();

        return services;
    }
}
