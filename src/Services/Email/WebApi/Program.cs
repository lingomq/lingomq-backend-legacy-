using Email.Application.Consumers;
using Email.Application.Contracts;
using Email.Application.Services;
using EventBus.Entities.Email;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .AddEnvironmentVariables();

// Services
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Add MassTransit
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddDelayedMessageScheduler();
    x.AddConsumer<EmailSignUpConsumer>();
    x.AddConsumer<EmailSignInConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Uri"]!, "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:UserName"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });

        cfg.ReceiveEndpoint(typeof(EmailModelSignUp).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<EmailSignUpConsumer>(context);
            endpoint.ConfigureConsumer<EmailSignInConsumer>(context);
        });
        cfg.ClearSerialization();
        cfg.UseRawJsonSerializer();
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

builder.Configuration
    .AddEnvironmentVariables();

app.UseHttpsRedirection();

app.Run();
