using FoodPal.Orders.BackgroundServices.Handlers.Contracts;
using FoodPal.Orders.DTOs;
using FoodPal.Orders.MessageBroker.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodPal.Orders.BackgroundWorkers.Workers
{
    public class NewOrderWorker : IHostedService
    {
        private readonly ILogger<NewOrderWorker> _logger;
        private readonly IMessageBroker _messageBroker;
        private readonly IMessageHandlerFactory _messageHandlerFactory;

        public NewOrderWorker(ILogger<NewOrderWorker> logger, IMessageBroker messageBroker, IMessageHandlerFactory messageHandlerFactory)
        {
            _logger = logger;
            _messageBroker = messageBroker;
            _messageHandlerFactory = messageHandlerFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{this.GetType().Name} starting and registering message handler.");
            _messageBroker.RegisterMessageReceiver("new-orders", HandleMessageAsync);
            await _messageBroker.StartListenerAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{this.GetType().Name} stopping.");
            await _messageBroker.StopListenerAsync();
        }

        private async Task HandleMessageAsync(string messageContent)
        {
            try
            {
                var payload = JsonConvert.DeserializeObject<MessageBrokerEnvelope<NewOrderDTO>>(messageContent);
                var handler = _messageHandlerFactory.GetHandler(MessageTypes.NewOrder);

                await handler.ExecuteAsync<NewOrderDTO>(payload);
            }
            catch(Exception ex)
            {
                throw new Exception("Message processing failed.", ex);
            }
        }
    }
}
