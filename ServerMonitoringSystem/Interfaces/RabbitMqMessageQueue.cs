using RabbitMQ.Client;
using System.Text;
using ServerMonitoringAndNotification.Models;
using ServerMonitoringAndNotification.MessageBrokerInterfaces;


namespace ServerMonitoringAndNotification.RabbitMQComponents
{
    public class RabbitMQMessageQueue : IMessageQueue, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RabbitMQConfig _rabbitMQConfig;
        private readonly ILogger<RabbitMQMessageQueue> _logger;


        public RabbitMQMessageQueue(RabbitMQConfig rabbitMQConfig, ILogger<RabbitMQMessageQueue> logger)
        {
            _rabbitMQConfig = rabbitMQConfig;
            _logger = logger;
            _logger.LogInformation($"RabbitMQ Configuration: Host={_rabbitMQConfig.HostName}, Port={_rabbitMQConfig.Port}, UserName={_rabbitMQConfig.UserName}");

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQConfig.HostName,
                Port = _rabbitMQConfig.Port,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            DeclareRabbitMQComponents();
        }

        private void DeclareRabbitMQComponents()
        {
            _channel.ExchangeDeclare(_rabbitMQConfig.ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(_rabbitMQConfig.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: _rabbitMQConfig.QueueName, exchange: _rabbitMQConfig.ExchangeName, routingKey: _rabbitMQConfig.RoutingKey);
        }

        public void Publish(string serverIdentifier, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _logger.LogInformation($"Message Sent: {message}");
            _channel.BasicPublish(exchange: serverIdentifier, routingKey: _rabbitMQConfig.RoutingKey, basicProperties: null, body: body);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
