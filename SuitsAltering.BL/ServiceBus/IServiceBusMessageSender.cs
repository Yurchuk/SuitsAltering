using SuitsAltering.Contracts;

namespace SuitsAltering.BL.ServiceBus;

public interface IServiceBusMessageSender
{
    Task OrderPaidSendAsync(OrderPaid orderPaid);
    Task OrderDoneSendAsync(OrderDone orderDone);
}