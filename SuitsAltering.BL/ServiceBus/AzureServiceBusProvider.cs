using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace SuitsAltering.BL.ServiceBus;

public class AzureServiceBusProvider: IServiceBusProvider, IServiceBusInitializer
{
    private readonly JsonSerializerSettings _serviceBusSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
    private readonly IOptionsSnapshot<AzureServiceBusSettings> _azureServiceBusSettingsSnapshot;
    private readonly ServiceBusClient _client;
    private readonly ILogger<AzureServiceBusProvider> _logger;

    public AzureServiceBusProvider(
        IOptionsSnapshot<AzureServiceBusSettings> azureServiceBusSettingsSnapshot, 
        ILogger<AzureServiceBusProvider> logger)
    {
        _azureServiceBusSettingsSnapshot = azureServiceBusSettingsSnapshot;
        _logger = logger;
        _client = new ServiceBusClient(azureServiceBusSettingsSnapshot.Value.ConnectionString);
        
    }

    public async Task SendAsync<T>(T message, string topicName)
    {
        var sender = _client.CreateSender(topicName);
        var serviceBusMessage = new ServiceBusMessage
        {
            MessageId = Guid.NewGuid().ToString(),
            CorrelationId = Guid.NewGuid().ToString(),
            SessionId = Guid.NewGuid().ToString(),
            ScheduledEnqueueTime = DateTimeOffset.UtcNow
        };

        try
        {
            var json = JsonConvert.SerializeObject(message, _serviceBusSerializerSettings);
            serviceBusMessage.Body = BinaryData.FromString(json);

            await sender.SendMessageAsync(serviceBusMessage);
            _logger.LogInformation("Message was sent");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Sending Message to Service Bus exception. MessageId: {serviceBusMessage.MessageId}");
            throw;
        }
    }

    public async Task InitializeAsync(string[] topicsNames)
    {
        foreach (var topicName in topicsNames)
        {
            try
            {
                var adminClient = new ServiceBusAdministrationClient(_azureServiceBusSettingsSnapshot.Value.ConnectionString);
                var isTopicExists = await adminClient.TopicExistsAsync(topicName);
                
                if (isTopicExists == false)
                {
                    await adminClient.CreateTopicAsync(topicName);
                }

                var topicSubscriptionName = $"{topicName}-subscription";
                var isTopicSubscriptionExists = await adminClient.SubscriptionExistsAsync(topicName, topicSubscriptionName);
                if (isTopicSubscriptionExists == false)
                {
                    await adminClient.CreateSubscriptionAsync(topicName, topicSubscriptionName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create topic due to: {ex}");
            }
        }
    }
}