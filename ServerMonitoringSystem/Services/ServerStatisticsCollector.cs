using System.Diagnostics;
using ServerMonitoringAndNotification.Models;

namespace ServerMonitoringAndNotification.ServerStatisticsServices
{
    public class ServerStatisticsCollector
    {
        public ServerStatistics CollectServerStatistics(string serverIdentifier)
        {
            var memoryUsage = GetMemoryUsageInMB();
            var availableMemory = GetAvailableMemoryInMB();
            var cpuUsage = GetCpuUsage();
            var timestamp = DateTime.Now;

            return new ServerStatistics
            {
                ServerIdentifier = serverIdentifier,
                MemoryUsage = memoryUsage,
                AvailableMemory = availableMemory,
                CpuUsage = cpuUsage,
                Timestamp = timestamp
            };
        }

        private double GetMemoryUsageInMB()
        {
            using (var currentProcess = Process.GetCurrentProcess())
            {
                return currentProcess.WorkingSet64 / (1024 * 1024);
            }
        }

        private double GetAvailableMemoryInMB()
        {
            using (var currentProcess = Process.GetCurrentProcess())
            {
                return currentProcess.PrivateMemorySize64 / (1024 * 1024);
            }
        }

        private double GetCpuUsage()
        {
            using (var currentProcess = Process.GetCurrentProcess())
            {
                return currentProcess.TotalProcessorTime.TotalSeconds;
            }
        }
    }
}