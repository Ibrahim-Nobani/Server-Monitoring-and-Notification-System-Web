using ServerMonitoringAndNotification.ServerStatisticsServices;
using ServerMonitoringAndNotification.MessageBrokerInterfaces;
using ServerMonitoringAndNotification.RabbitMQComponents;
using Microsoft.Extensions.DependencyInjection;
using ServerMonitoringAndNotification.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using ServerMonitoringAndNotification.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddSingleton<ServerStatisticsCollector>();
builder.Services.AddSingleton<RabbitMQConfig>(builder.Configuration.GetSection("RabbitMQConfig").Get<RabbitMQConfig>());


builder.Services.AddSingleton<IMessageQueue, RabbitMQMessageQueue>();
builder.Services.AddSingleton<IMessageReceiver, RabbitMQMessageReceiver>();

builder.Services.AddSignalR();

builder.Services.AddSingleton<AnomalyDetectionConfig>(builder.Configuration.GetSection("AnomalyDetectionConfig").Get<AnomalyDetectionConfig>());
builder.Services.AddSingleton<AnomalyDetector>();


builder.Services.Configure<MongoDBConfig>(builder.Configuration.GetSection("MongoDBConfig"));
builder.Services.AddSingleton<IMongoDatabase>(provider =>
{
    var config = provider.GetRequiredService<IOptions<MongoDBConfig>>().Value;
    var client = new MongoClient(config.ConnectionString);
    return client.GetDatabase(config.DatabaseName);
});

builder.Services.AddSingleton<IRepository<ServerStatistics>, ServerStatisticsRepository>();

builder.Services.AddSingleton<ServerStatisticsPublisher>();
builder.Services.AddSingleton<ServerStatisticsReceiver>();



builder.Services.AddHostedService<ServerStatisticsPublisherService>();
builder.Services.AddHostedService<ServerStatisticsReceiverService>();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Application starting.");

app.MapHub<AlertHub>("/alerthub");

app.Run();

public class ServerStatisticsPublisherService : BackgroundService
{
    private readonly ServerStatisticsPublisher _serverStatisticsPublisher;

    public ServerStatisticsPublisherService(ServerStatisticsPublisher serverStatisticsPublisher)
    {
        _serverStatisticsPublisher = serverStatisticsPublisher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _serverStatisticsPublisher.StartAsync();
    }
}

public class ServerStatisticsReceiverService : BackgroundService
{
    private readonly ServerStatisticsReceiver _serverStatisticsReceiver;

    public ServerStatisticsReceiverService(ServerStatisticsReceiver serverStatisticsReceiver)
    {
        _serverStatisticsReceiver = serverStatisticsReceiver;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _serverStatisticsReceiver.StartReceivingMessagesAsync();
    }
}