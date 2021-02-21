using FoodPal.Orders.MessageBroker.Contracts;
using System.Threading.Tasks;

namespace FoodPal.Orders.BackgroundServices.Handlers.Contracts
{
    public interface IMessageHandler
    {
        Task ExecuteAsync<TPayload>(MessageBrokerEnvelope<TPayload> messageEnvelope);
    }
}
