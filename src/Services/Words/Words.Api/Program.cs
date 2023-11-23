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
using Words.Api.Services;
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
    loggingBuilder.AddNLog();
});

// Data layer
builder.Services.AddScoped<IDbConnection>((sp) => new NpgsqlConnection(builder.Configuration["ConnectionStrings:Dev:Words"]));
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserWordRepository, UserWordRepository>();
builder.Services.AddScoped<IUserWordTypeRepository, UserWordTypeRepository>();
builder.Services.AddScoped<IWordTypeRepository, WordTypeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IDatabaseDataMigrator, DatabaseDataMigrator>();
builder.Services.AddScoped<PublisherBase>();

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
        .WithGlobalConnectionString(builder.Configuration["ConnectionStrings:Dev:Words"])
        .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

// MassTransin
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<WordsDeleteUserConsumer>();
    x.AddConsumer<WordsUpdateUserConsumer>();
    x.AddConsumer<WordsCreateUserConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Uri"]!, "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:UserName"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });

        cfg.ReceiveEndpoint(typeof(WordsDeleteUserConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<WordsDeleteUserConsumer>(context);
        }); 
        cfg.ReceiveEndpoint(typeof(WordsUpdateUserConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<WordsUpdateUserConsumer>(context);
        });
        cfg.ReceiveEndpoint(typeof(WordsCreateUserConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<WordsCreateUserConsumer>(context);
        });
        cfg.ClearSerialization();
        cfg.UseRawJsonSerializer();
        cfg.ConfigureEndpoints(context);
        cfg.Publish<PublisherBase>();
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

app.UseMiddleware<ExceptionsCatchingMiddleware>();

// Auto migrations
using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var runner = services.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();

    var dataMigrator = services.GetRequiredService<IDatabaseDataMigrator>();
    await dataMigrator.DoWork();
}

app.Run();
