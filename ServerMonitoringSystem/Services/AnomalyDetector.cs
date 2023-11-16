using Microsoft.AspNetCore.SignalR;
using ServerMonitoringAndNotification.Models;

namespace ServerMonitoringAndNotification.ServerStatisticsServices
{
    public class AnomalyDetector
    {
        private readonly AnomalyDetectionConfig _anomalyDetectionConfig;
        private readonly IHubContext<AlertHub> _alertHubContext;
        private ServerStatistics? previousStatistics;

        public AnomalyDetector(AnomalyDetectionConfig anomalyDetectionConfig, IHubContext<AlertHub> alertHubContext)
        {
            _anomalyDetectionConfig = anomalyDetectionConfig;
            _alertHubContext = alertHubContext;
        }

        public async Task<bool> DetectAnomalies(ServerStatistics currentStatistics, string serverIdentifier)
        {
            if (previousStatistics == null)
            {
                previousStatistics = currentStatistics;
                return false;
            }

            if (currentStatistics.MemoryUsage > (previousStatistics.MemoryUsage * (1 + _anomalyDetectionConfig.MemoryUsageAnomalyThresholdPercentage)))
            {
                await _alertHubContext.Clients.All.SendAsync("SendMessage", AlertType.MemoryUsageAnomalyAlert);
                previousStatistics = currentStatistics;
                return true;
            }

            if (currentStatistics.CpuUsage > (previousStatistics.CpuUsage * (1 + _anomalyDetectionConfig.CpuUsageAnomalyThresholdPercentage)))
            {
                await _alertHubContext.Clients.All.SendAsync("SendMessage", AlertType.CpuUsageAnomalyAlert);
                previousStatistics = currentStatistics;
                return true;
            }

            if ((currentStatistics.MemoryUsage / (currentStatistics.MemoryUsage + currentStatistics.AvailableMemory)) > _anomalyDetectionConfig.MemoryUsageThresholdPercentage)
            {
                await _alertHubContext.Clients.All.SendAsync("SendMessage", AlertType.MemoryHighUsageAlert);
                previousStatistics = currentStatistics;
                return true;
            }

            if (currentStatistics.CpuUsage > _anomalyDetectionConfig.CpuUsageThresholdPercentage)
            {
                await _alertHubContext.Clients.All.SendAsync("SendMessage", AlertType.CpuHighUsageAlert);
                previousStatistics = currentStatistics;
                return true;
            }

            previousStatistics = currentStatistics;
            return false;
        }
    }
}