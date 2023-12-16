﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Authentication.Application.EventBus.MassTransit.Consumers;
using EventBus.Entities.Identity.User;

namespace Authentication.Application.EventBus.MassTransit;
public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
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

                cfg.ReceiveEndpoint(typeof(IdentityModelDeleteUser).Name.ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<DeleteUserConsumer>(context);
                });

                cfg.ReceiveEndpoint(typeof(IdentityModelUpdateUserCredentials).Name.ToLower(), endpoint =>
                {
                    endpoint.ConfigureConsumer<UpdateUserCredentialsConsumer>(context);
                });

                cfg.ReceiveEndpoint(typeof(IdentityModelUpdateUser).Name.ToLower(), endpoint =>
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