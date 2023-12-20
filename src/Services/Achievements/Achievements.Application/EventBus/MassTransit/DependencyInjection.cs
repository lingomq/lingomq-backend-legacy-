using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Achievements.Application.EventBus.MassTransit.Consumers;
using MassTransit;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddDelayedMessageScheduler();
            x.AddConsumer<DeleteUserConsumer>();
            x.AddConsumer<UpdateUserConsumer>();
            x.AddConsumer<CreateUserConsumer>();
            x.AddConsumer<CheckAchievementsConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Uri"]!, "/", h =>
                {
                    h.Username(configuration["RabbitMq:UserName"]);
                    h.Password(configuration["RabbitMq:Password"]);
                });

                cfg.ReceiveEndpoint("achievements_" + nameof(DeleteUserConsumer).ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<DeleteUserConsumer>(context);
                });
                cfg.ReceiveEndpoint("achievements_" + nameof(UpdateUserConsumer).ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<UpdateUserConsumer>(context);
                });

                cfg.ReceiveEndpoint("achievements_" + nameof(CreateUserConsumer).ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<CreateUserConsumer>(context);
                });

                cfg.ReceiveEndpoint("achievements_" + nameof(CheckAchievementsConsumer).ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<CheckAchievementsConsumer>(context);
                });

                cfg.ClearSerialization();
                cfg.UseRawJsonSerializer();
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
