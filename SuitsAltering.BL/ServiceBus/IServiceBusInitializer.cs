namespace SuitsAltering.BL.ServiceBus;

public interface IServiceBusInitializer
{
    Task InitializeAsync(string[] topicsNames);
}