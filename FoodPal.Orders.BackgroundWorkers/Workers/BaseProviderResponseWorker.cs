using FoodPal.Orders.BackgroundServices.Handlers.Contracts;
using FoodPal.Orders.MessageBroker.Contracts;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace FoodPal.Orders.BackgroundWorkers.Workers
{
	public abstract class BaseProviderResponseWorker
	{
		private readonly ILogger _logger;
		protected readonly IMessageBroker MessageBroker;
		protected readonly IMessageHandlerFactory MessageHandlerFactory;

		protected BaseProviderResponseWorker(ILogger logger, IMessageBroker messageBroker, IMessageHandlerFactory messageHandlerFactory)
		{
			_logger = logger;
			MessageBroker = messageBroker;
			MessageHandlerFactory = messageHandlerFactory;
		}

		protected async Task StartAsync(string providerName, MessageReceivedEventHandler eventHandler, CancellationToken cancellationToken)
		{
			_logger.LogDebug($"{this.GetType().Name} starting; registering message handler.");
			MessageBroker.RegisterMessageReceiver($"provider-{providerName}-response", eventHandler);
			await MessageBroker.StartListenerAsync();
		}

		protected async Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug($"{this.GetType().Name} stopping.");
			await MessageBroker.StopListenerAsync();
		}
	}
}
