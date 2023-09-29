using EventBus.Entities.Email;
using Identity.BusinessLayer.Contracts;
using Identity.BusinessLayer.MassTransit.Consumers;
using Identity.BusinessLayer.Services;
using Identity.BusinessLayer.Services.Repositories;
using MassTransit;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IDbConnection>(
    (sp) => new NpgsqlConnection(builder.Configuration["ConnectionStrings:Dev"]));
builder.Services.AddTransient<ILinkTypeRepository, LinkTypeRepository>();
builder.Services.AddTransient<IUserInfoRepository, UserInfoRepository>();
builder.Services.AddTransient<IUserLinkRepository, UserLinkRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddTransient<IUserStatisticsRepository, UserStatisticsRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddDelayedMessageScheduler();
    x.AddConsumer<IdentityCreateUserConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:UserName"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });

        cfg.ReceiveEndpoint(typeof(EmailModelSignUp).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<IdentityCreateUserConsumer>(context);
        });
        cfg.ClearSerialization();
        cfg.UseRawJsonSerializer();
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
