using Authentication.Application.EventBus.MassTransit;
using Authentication.Application.EventBus.MassTransit.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<PublisherBase>();
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumer<DeleteUserConsumer>();
            x.AddConsumer<UpdateUserCredentialsConsumer>();
            x.AddConsumer<UpdateUserConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {

                cfg.Host(configuration["RabbitMq:Uri"]!, "/", h =>
                {
                    h.Username(configuration["RabbitMq:UserName"]);
                    h.Password(configuration["RabbitMq:Password"]);
                });

                cfg.ReceiveEndpoint("authentication_" + typeof(DeleteUserConsumer).Name.ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<DeleteUserConsumer>(context);
                });

                cfg.ReceiveEndpoint("authentication_" + typeof(UpdateUserCredentialsConsumer).Name.ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<UpdateUserCredentialsConsumer>(context);
                });

                cfg.ReceiveEndpoint("authentication_" + typeof(UpdateUserConsumer).Name.ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<UpdateUserConsumer>(context);
                });

                cfg.ClearSerialization();
                cfg.UseRawJsonSerializer();
                cfg.ConfigureEndpoints(context);
                cfg.Publish<PublisherBase>();
            });
        });

        return services;
    }
}
