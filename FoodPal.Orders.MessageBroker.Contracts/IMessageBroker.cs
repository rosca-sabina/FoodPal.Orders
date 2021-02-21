using System.Threading.Tasks;

namespace FoodPal.Orders.MessageBroker.Contracts
{
    public delegate Task MessageReceivedEventHandler(string messageContent);

    public interface IMessageBroker
    {
        Task SendMessageAsync<TMessage>(string queueName, TMessage message);
        void RegisterMessageReceiver(string queueName, MessageReceivedEventHandler messageHandler);
        Task StartListenerAsync();
        Task StopListenerAsync();
    }
}
