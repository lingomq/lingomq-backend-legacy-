using MassTransit;
using Microsoft.Extensions.Configuration;
using Topics.BusinessLayer.MassTransit.Consumers.IdentityConsumers;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class DependencyInjection
{
    public static IServiceCollection AddApplicationMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        // MassTransit
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddDelayedMessageScheduler();
            x.AddConsumer<TopicsDeleteUserConsumer>();
            x.AddConsumer<TopicsUpdateUserConsumer>();
            x.AddConsumer<TopicsCreateUserConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Uri"]!, "/", h =>
                {
                    h.Username(configuration["RabbitMq:UserName"]);
                    h.Password(configuration["RabbitMq:Password"]);
                });

                cfg.ReceiveEndpoint(typeof(TopicsDeleteUserConsumer).Name.ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<TopicsDeleteUserConsumer>(context);
                });
                cfg.ReceiveEndpoint(typeof(TopicsUpdateUserConsumer).Name.ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<TopicsUpdateUserConsumer>(context);
                });
                cfg.ReceiveEndpoint(typeof(TopicsCreateUserConsumer).Name.ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<TopicsCreateUserConsumer>(context);
                });
                cfg.ClearSerialization();
                cfg.UseRawJsonSerializer();
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}