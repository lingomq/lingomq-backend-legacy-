using FluentMigrator.Runner;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Npgsql;
using System.Data;
using System.Reflection;
using System.Text;
using Words.Api.Middlewares;
using Words.BusinessLayer.Contracts;
using Words.BusinessLayer.MassTransit;
using Words.BusinessLayer.MassTransit.Consumers;
using Words.BusinessLayer.Services;
using Words.BusinessLayer.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .AddEnvironmentVariables();

// Add Logging (NLog)
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddNLog();
});

// Data layer
builder.Services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(builder.Configuration["ConnectionStrings:Dev"]));
builder.Services.AddTransient<ILanguageRepository, LanguageRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserWordRepository, UserWordRepository>();
builder.Services.AddTransient<IUserWordTypeRepository, UserWordTypeRepository>();
builder.Services.AddTransient<IWordTypeRepository, WordTypeRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<PublisherBase>();

// Other services
builder.Services.AddTransient<IWordChecker, WordChecker>();

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
        .WithGlobalConnectionString(builder.Configuration["ConnectionStrings:Dev"])
        .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

// MassTransin
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddDelayedMessageScheduler();
    x.AddConsumer<IdentityDeleteUserConsumer>();
    x.AddConsumer<IdentityUpdateUserConsumer>();
    x.AddConsumer<IdentityCreateUserConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:UserName"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });

        cfg.ReceiveEndpoint(typeof(IdentityDeleteUserConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<IdentityDeleteUserConsumer>(context);
        }); 
        cfg.ReceiveEndpoint(typeof(IdentityUpdateUserConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<IdentityUpdateUserConsumer>(context);
        });
        cfg.ReceiveEndpoint(typeof(IdentityCreateUserConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<IdentityCreateUserConsumer>(context);
        });
        cfg.ClearSerialization();
        cfg.Publish<PublisherBase>();
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
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionsCatchingMiddleware>();

// Auto migrations

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var runner = services.GetRequiredService<IMigrationRunner>();

    runner.MigrateUp();
}

app.Run();
