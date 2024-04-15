namespace ServerMonitoringAndNotification.Models
{
    public class RabbitMQConfig
    {
        public string HostName { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
