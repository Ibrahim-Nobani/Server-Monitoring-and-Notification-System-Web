using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using ServerMonitoringAndNotification.Models;
using ServerMonitoringAndNotification.MessageBrokerInterfaces;

namespace ServerMonitoringAndNotification.RabbitMQComponents
{
    public class RabbitMQMessageReceiver : IMessageReceiver, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RabbitMQConfig _rabbitMQConfig;

        public RabbitMQMessageReceiver(RabbitMQConfig rabbitMQConfig)
        {
            _rabbitMQConfig = rabbitMQConfig;

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

        public void StartReceivingMessages(Action<string> messageHandler)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                messageHandler(message);
                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: _rabbitMQConfig.QueueName, autoAck: false, consumer: consumer);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}