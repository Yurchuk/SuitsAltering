using SuitsAltering.Contracts;

namespace SuitsAltering.BL.ServiceBus;

public class ServiceBusMessageSender : IServiceBusMessageSender
{
    public static string[] Topics = new[] { OrderPaidTopic, OrderDoneTopic };
    private readonly IServiceBusProvider _serviceBusProvider;
    private const string OrderPaidTopic = "order-paid";
    private const string OrderDoneTopic = "order-done";
    public ServiceBusMessageSender(
        IServiceBusProvider serviceBusProvider)
    {
        _serviceBusProvider = serviceBusProvider;
    }
    public Task OrderPaidSendAsync(OrderPaid orderPaid)
    {
        return _serviceBusProvider.SendAsync(orderPaid, OrderPaidTopic);
    }

    public Task OrderDoneSendAsync(OrderDone orderDone)
    {
        return _serviceBusProvider.SendAsync(orderDone, OrderDoneTopic);
    }
}