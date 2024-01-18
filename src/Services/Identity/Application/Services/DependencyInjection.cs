using Microsoft.Extensions.Configuration;
using Identity.Domain.Contracts;
using Identity.Application.Services.Authentication;
using Identity.Application.Services.Confirmation;
using Identity.Application.Services.UserActions;
using Identity.Application.Services.Jwt;

namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyInjection
{
    public static IServiceCollection AddPostgresDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IConfirmationService, ConfirmationService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
