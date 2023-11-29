using System.Data;
using System.Reflection;
using System.Text;
using FluentMigrator.Runner;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Notifications.Api.Middlewares;
using Notifications.Api.Services;
using Notifications.BusinessLayer.Contracts;
using Notifications.BusinessLayer.MassTransit.Consumers;
using Notifications.BusinessLayer.Services;
using Notifications.BusinessLayer.Services.Repositories;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .AddEnvironmentVariables();

// Add Logging (NLog)
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddNLog();
});

// DataAccess
builder.Services.AddTransient<IDbConnection>((sp) =>
    new NpgsqlConnection(builder.Configuration["ConnectionStrings:Dev:Notification"]));
builder.Services.AddTransient<INotificationRepository, NotificationRepository>();
builder.Services.AddTransient<INotificationTypeRepository, NotificationTypeRepository>();
builder.Services.AddTransient<IUserNotificationRepository, UserNotificationRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IDatabaseDataMigrator, DatabaseDataMigrator>();

// Authentication (current: JWT)
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
        };
    });

// Migrations
builder.Services.AddFluentMigratorCore()
        .ConfigureRunner(cr => cr
        .AddPostgres()
        .WithGlobalConnectionString(builder.Configuration["ConnectionStrings:Dev:Notification"])
        .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddDelayedMessageScheduler();
    x.AddConsumer<NotificationsDeleteUserConsumer>();
    x.AddConsumer<NotificationsUpdateUserConsumer>();
    x.AddConsumer<NotificationsCreateUserConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Uri"]!, "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:UserName"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });

        cfg.ReceiveEndpoint(typeof(NotificationsDeleteUserConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<NotificationsDeleteUserConsumer>(context);
        });
        cfg.ReceiveEndpoint(typeof(NotificationsUpdateUserConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<NotificationsUpdateUserConsumer>(context);
        });
        cfg.ReceiveEndpoint(typeof(NotificationsCreateUserConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<NotificationsCreateUserConsumer>(context);
        });
        cfg.ClearSerialization();
        cfg.UseRawJsonSerializer();
        cfg.ConfigureEndpoints(context);
    });
});

// Swagger Auth interface
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "Jwt",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
             new string[] {}
          }
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

builder.Configuration
    .AddEnvironmentVariables();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var runner = services.GetRequiredService<IMigrationRunner>();
    var databaseMigrator = services.GetRequiredService<IDatabaseDataMigrator>();
    runner.MigrateUp();
    await databaseMigrator.Migrate();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
