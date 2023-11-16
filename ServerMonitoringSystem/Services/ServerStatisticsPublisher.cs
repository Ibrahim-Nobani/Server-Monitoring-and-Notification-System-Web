using Newtonsoft.Json;
using ServerMonitoringAndNotification.MessageBrokerInterfaces;
using ServerMonitoringAndNotification.Models;
using ServerMonitoringAndNotification.ServerStatisticsServices;

namespace ServerMonitoringAndNotification.ServerStatisticsServices
{
    public class ServerStatisticsPublisher
    {
        private readonly IConfiguration _configuration;
        private readonly IMessageQueue _messageQueue;
        private readonly ServerStatisticsCollector _serverStatisticsCollection;


        public ServerStatisticsPublisher(IConfiguration configuration, IMessageQueue messageQueue, ServerStatisticsCollector serverStatisticsCollection)
        {
            _configuration = configuration;
            _messageQueue = messageQueue;
            _serverStatisticsCollection = serverStatisticsCollection;
        }

        public async Task StartAsync()
        {
            var samplingIntervalSeconds = _configuration.GetValue<int>("ServerStatisticsConfig:SamplingIntervalSeconds");
            var serverIdentifier = _configuration["ServerStatisticsConfig:ServerIdentifier"];

            while (true)
            {
                var statistics = _serverStatisticsCollection.CollectServerStatistics(serverIdentifier);
                PublishStatistics(serverIdentifier, statistics);

                await Task.Delay(TimeSpan.FromSeconds(samplingIntervalSeconds));
            }
        }

        private void PublishStatistics(string serverIdentifier, ServerStatistics statistics)
        {
            var message = JsonConvert.SerializeObject(statistics);
            _messageQueue.Publish(serverIdentifier, message);
        }
    }
}