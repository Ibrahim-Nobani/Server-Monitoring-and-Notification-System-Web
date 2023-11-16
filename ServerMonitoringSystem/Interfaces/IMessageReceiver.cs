namespace ServerMonitoringAndNotification.MessageBrokerInterfaces
{
    public interface IMessageReceiver
    {
        void StartReceivingMessages(Action<string> messageHandler);
    }
}