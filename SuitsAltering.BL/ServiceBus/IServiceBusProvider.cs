namespace SuitsAltering.BL.ServiceBus;

public interface IServiceBusProvider
{
    Task SendAsync<T>(T message, string topicName);
}