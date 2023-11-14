using Newtonsoft.Json;
using ServerMonitoringAndNotification.MessageBrokerInterfaces;
using ServerMonitoringAndNotification.Models;
using ServerMonitoringAndNotification.RabbitMQComponents;
using MongoDB.Driver;
using ServerMonitoringAndNotification.Repositories;

namespace ServerMonitoringAndNotification.ServerStatisticsServices
{
    public class ServerStatisticsReceiver
    {
        private readonly IMessageReceiver _messageReceiver;
        private readonly IRepository<ServerStatistics> _serverStatisticsRepository;
        private readonly AnomalyDetector _anomalyDetector;
        private readonly ILogger<ServerStatisticsReceiver> _logger;


        public ServerStatisticsReceiver(IMessageReceiver messageReceiver, IRepository<ServerStatistics> serverStatisticsRepository, AnomalyDetector anomalyDetector, ILogger<ServerStatisticsReceiver> logger)
        {
            _messageReceiver = messageReceiver;
            _serverStatisticsRepository = serverStatisticsRepository;
            _anomalyDetector = anomalyDetector;
            _logger = logger;
        }

        public async Task StartReceivingMessagesAsync()
        {
            while (true)
            {
                _messageReceiver.StartReceivingMessages(HandleReceivedMessage);
                await Task.Delay(TimeSpan.FromSeconds(12));
            }
        }

        private async void HandleReceivedMessage(string message)
        {
            var deserializedServerStatisticsObject = JsonConvert.DeserializeObject<ServerStatistics>(message);
            _serverStatisticsRepository.Add(deserializedServerStatisticsObject);

            if (await _anomalyDetector.DetectAnomalies(deserializedServerStatisticsObject, deserializedServerStatisticsObject.ServerIdentifier))
            {
                _logger.LogInformation("Anomaly Detected");
            }

            _logger.LogInformation("Received message: " + message);

        }
    }
}
