using Achievements.Application.MassTransit;
using Achievements.BusinessLayer.MassTransit.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyInjection
{
    public static IServiceCollection AddApplicationMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<PublisherBase>();
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddDelayedMessageScheduler();
            x.AddConsumer<CheckConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Uri"]!, "/", h =>
                {
                    h.Username(configuration["RabbitMq:UserName"]);
                    h.Password(configuration["RabbitMq:Password"]);
                });

                cfg.ReceiveEndpoint("achievements_" + typeof(CheckConsumer).Name.ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<CheckConsumer>(context);
                });

                cfg.ClearSerialization();
                cfg.UseRawJsonSerializer();
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
