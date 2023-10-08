using AppStatistics.BusinessLayer.Contracts.DataAccess;
using AppStatistics.BusinessLayer.Contracts.Service;
using AppStatistics.BusinessLayer.MassTransit.Consumers;
using AppStatistics.BusinessLayer.Services;
using AppStatistics.BusinessLayer.Services.DataAccess;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");

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
        cfg.Host("localhost", "/", h =>
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
