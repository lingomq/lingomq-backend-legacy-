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
            x.AddConsumer<DeleteUserConsumer>();
            x.AddConsumer<UpdateUserConsumer>();
            x.AddConsumer<CreateUserConsumer>();
            x.AddConsumer<CheckConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Uri"]!, "/", h =>
                {
                    h.Username(configuration["RabbitMq:UserName"]);
                    h.Password(configuration["RabbitMq:Password"]);
                });

                cfg.ReceiveEndpoint("achievements_" + typeof(DeleteUserConsumer).Name.ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<DeleteUserConsumer>(context);
                });
                cfg.ReceiveEndpoint("achievements_" + typeof(UpdateUserConsumer).Name.ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<UpdateUserConsumer>(context);
                });

                cfg.ReceiveEndpoint("achievements_" + typeof(CreateUserConsumer).Name.ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<CreateUserConsumer>(context);
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
