using FoodPal.Orders.DTOs;
using FoodPal.Orders.MessageBroker.Contracts;
using FoodPal.Orders.Services.Contracts;
using System.Threading.Tasks;

namespace FoodPal.Orders.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IMessageBroker _messageBroker;
        public OrderService(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        public async Task<string> CreateAsync(NewOrderDTO newOrder)
        {
            ValidateNewOrder(newOrder);

            var payload = new MessageBrokerEnvelope<NewOrderDTO>(MessageTypes.NewOrder, newOrder);
            await _messageBroker.SendMessageAsync("new-orders", payload);

            return payload.RequestId;
        }


        private void ValidateNewOrder(NewOrderDTO newOrder)
        {
            // TODO
        }
    }
}
