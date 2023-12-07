using Achievements.Api.Middlewares;
using Achievements.BusinessLayer.Contracts;
using Achievements.BusinessLayer.MassTransit.Consumers;
using Achievements.BusinessLayer.Services;
using Achievements.BusinessLayer.Services.Repositories;
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
using Achievements.Api.Services;

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

// Data layer
builder.Services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(builder.Configuration["ConnectionStrings:Dev:Achievements"]));
builder.Services.AddTransient<IAchievementRepository, AchievementRepository>();
builder.Services.AddTransient<IUserAchievementRepository, UserAchievementRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IDatabaseDataMigrator, DatabaseDataMigrator>();

// Authentication
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
        .WithGlobalConnectionString(builder.Configuration["ConnectionStrings:Dev:Achievements"])
        .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mass Transit
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddDelayedMessageScheduler();
    x.AddConsumer<AchievementsDeleteUserConsumer>();
    x.AddConsumer<AchievementsUpdateUserConsumer>();
    x.AddConsumer<AchievementsCreateUserConsumer>();
    x.AddConsumer<AchievementCheckConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Uri"]!, "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:UserName"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });

        cfg.ReceiveEndpoint(typeof(AchievementsDeleteUserConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<AchievementsDeleteUserConsumer>(context);
        });
        cfg.ReceiveEndpoint(typeof(AchievementsUpdateUserConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<AchievementsUpdateUserConsumer>(context);
        });
        
        cfg.ReceiveEndpoint(typeof(AchievementsCreateUserConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<AchievementsCreateUserConsumer>(context);
        });

        cfg.ReceiveEndpoint(typeof(AchievementCheckConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<AchievementCheckConsumer>(context);
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

var app = builder.Build();

builder.Configuration
    .AddEnvironmentVariables();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlerMiddleware>();

// Auto migrations
using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var runner = services.GetRequiredService<IMigrationRunner>();
    var dataMigrator = services.GetRequiredService<IDatabaseDataMigrator>();
    runner.MigrateUp();

    await dataMigrator.MigrateAsync();
}

app.Run();
