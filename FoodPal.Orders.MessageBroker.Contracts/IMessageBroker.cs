using System.Threading.Tasks;

namespace FoodPal.Orders.MessageBroker.Contracts
{
    public interface IMessageBroker
    {
        Task SendMessageAsync<TMessage>(string queueName, TMessage message);
    }
}
