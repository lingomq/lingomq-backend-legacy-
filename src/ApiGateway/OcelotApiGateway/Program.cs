using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

string localAllowSpecificOrigins = "lan_allow_specific_origins";

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("ocelot.Development.json")
    .Build();

// Add services
builder.Services.AddCors(options =>
{
    options.AddPolicy(localAllowSpecificOrigins, policy =>
    {
        policy
        //policy.WithOrigins("https://192.168.0.101:9000", "https://localhost:9000", "https://localhost")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed((hosts) => true);
    });
});

builder.Services.AddOcelot(configuration);

var app = builder.Build();

app.UseCors(localAllowSpecificOrigins);
await app.UseOcelot();
app.UseAuthorization();

app.Run();
