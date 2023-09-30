using Authentication.Api.Middlewares;
using Authentication.BusinessLayer.Contracts;
using Authentication.BusinessLayer.MassTransit;
using Authentication.BusinessLayer.Services;
using Authentication.BusinessLayer.Services.Repositories;
using FluentMigrator.Runner;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;
using System.Reflection;
using System.Text;
using Authentication.BusinessLayer.MassTransit.Consumers;
using EventBus.Entities.Identity.User;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IDbConnection>(
    (sp) => new NpgsqlConnection(builder.Configuration["ConnectionStrings:Dev"]));
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddTransient<IUserInfoRepository, UserInfoRepository>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<Publisher>();
builder.Services.AddAuthentication(x => {
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

builder.Services.AddAuthorization();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddDelayedMessageScheduler();
    x.AddConsumer<IdentityDeleteUserConsumer>();
    x.AddConsumer<IdentityUpdateUserCredentialsConsumer>();
    x.AddConsumer<IdentityUpdateUserConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:UserName"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });
        
        cfg.ReceiveEndpoint(typeof(IdentityModelDeleteUser).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<IdentityDeleteUserConsumer>(context);
        });

        cfg.ReceiveEndpoint(typeof(IdentityModelUpdateUserCredentials).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<IdentityUpdateUserCredentialsConsumer>(context);
        });
        
        cfg.ReceiveEndpoint(typeof(IdentityModelUpdateUser).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<IdentityUpdateUserConsumer>(context);
        });
        
        cfg.ClearSerialization();
        cfg.Publish<Publisher>();
        cfg.UseRawJsonSerializer();
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddFluentMigratorCore()
        .ConfigureRunner(cr => cr
        .AddPostgres()
        .WithGlobalConnectionString(builder.Configuration["ConnectionStrings:Dev"])
        .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionsCathingMiddleware>();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var runner = services.GetRequiredService<IMigrationRunner>();

    runner.MigrateUp();
}

app.Run();
