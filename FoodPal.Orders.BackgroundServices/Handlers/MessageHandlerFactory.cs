using FoodPal.Orders.BackgroundServices.Handlers.Contracts;
using FoodPal.Orders.MessageBroker.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FoodPal.Orders.BackgroundServices.Handlers
{
    public class MessageHandlerFactory : IMessageHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public MessageHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IMessageHandler GetHandler(string messageType)
        {
            switch (messageType)
            {
                case MessageTypes.NewOrder:
                    return _serviceProvider.GetRequiredService<NewOrderMessageHandler>();
                case MessageTypes.OrderItemsProcessedByProvider:
                    return _serviceProvider.GetRequiredService<ProviderProcessedOrderItemsHandler>();
                default:
                    throw new Exception("Could not identify message handler for given message type.");
            }
        }
    }
}
