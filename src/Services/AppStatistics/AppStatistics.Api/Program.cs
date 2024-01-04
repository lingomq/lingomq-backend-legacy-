using AppStatistics.BusinessLayer.Contracts.DataAccess;
using AppStatistics.BusinessLayer.Contracts.Service;
using AppStatistics.BusinessLayer.MassTransit.Consumers;
using AppStatistics.BusinessLayer.Services;
using AppStatistics.BusinessLayer.Services.DataAccess;
using MassTransit;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true);

// Add Logging (NLog)
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddNLog();
});

// DataLayer
builder.Services.AddTransient<IMongoDbFactory>((sp) => new MongoDbFactory(builder.Configuration["ConnectionStrings:MongoDb"]!));
builder.Services.AddTransient<IAppStatisticsDataAccessService, AppStatisticsDataAccessService>();
builder.Services.AddTransient<IAppStatisticsService, AppStatisticsService>();

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddDelayedMessageScheduler();
    x.AddConsumer<AppStatisticsCreateOrUpdateConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Uri"]!, "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:UserName"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });

        cfg.ReceiveEndpoint(typeof(AppStatisticsCreateOrUpdateConsumer).Name.ToLower(), endpoint =>
        {
            endpoint.ConfigureConsumer<AppStatisticsCreateOrUpdateConsumer>(context);
        });
        cfg.ClearSerialization();
        cfg.UseRawJsonSerializer();
        cfg.ConfigureEndpoints(context);
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

app.Run();
