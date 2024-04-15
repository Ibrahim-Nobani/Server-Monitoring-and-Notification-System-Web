namespace ServerMonitoringAndNotification.MessageBrokerInterfaces
{
    public interface IMessageQueue
    {
        void Publish(string serverIdentifier, string message);
    }
}