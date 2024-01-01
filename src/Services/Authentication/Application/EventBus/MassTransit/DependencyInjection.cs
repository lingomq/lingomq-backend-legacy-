using Microsoft.Extensions.Configuration;
using MassTransit;
using Authentication.Application.EventBus.MassTransit.Consumers;
using EventBus.Entities.Identity.User;
using Authentication.Application.EventBus.MassTransit;

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

<<<<<<< HEAD
                cfg.ReceiveEndpoint(typeof(IdentityModelDeleteUser).Name.ToLower(), endpoint =>
=======
                cfg.ReceiveEndpoint("authentication_" + typeof(DeleteUserConsumer).Name.ToLower(), endpoint =>
>>>>>>> 999e945 (Update receive endpoints on authentication api)
                {
                    endpoint.ConfigureConsumer<DeleteUserConsumer>(context);
                });

<<<<<<< HEAD
                cfg.ReceiveEndpoint(typeof(IdentityModelUpdateUserCredentials).Name.ToLower(), endpoint =>
=======
                cfg.ReceiveEndpoint("authentication_" + typeof(UpdateUserCredentialsConsumer).Name.ToLower(), endpoint =>
>>>>>>> 999e945 (Update receive endpoints on authentication api)
                {
                    endpoint.ConfigureConsumer<UpdateUserCredentialsConsumer>(context);
                });

<<<<<<< HEAD
                cfg.ReceiveEndpoint(typeof(IdentityModelUpdateUser).Name.ToLower(), endpoint =>
=======
                cfg.ReceiveEndpoint("authentication_" + typeof(UpdateUserConsumer).Name.ToLower(), endpoint =>
>>>>>>> 999e945 (Update receive endpoints on authentication api)
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
