using FoodPal.Orders.BackgroundServices.Handlers.Contracts;
using FoodPal.Orders.DTOs.External;
using FoodPal.Orders.MessageBroker.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodPal.Orders.BackgroundWorkers.Workers
{
    public class XyzProviderOrderItemsWorker : BaseProviderResponseWorker, IHostedService
	{
		public XyzProviderOrderItemsWorker(ILogger<KfcProviderOrderItemsWorker> logger, IMessageBroker messageBroker, IMessageHandlerFactory messageHandlerFactory) : base(logger, messageBroker, messageHandlerFactory)
		{
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await base.StartAsync("xyz", HandleNextMessageAsync, cancellationToken);
		}

		public new async Task StopAsync(CancellationToken cancellationToken)
		{
			await base.StopAsync(cancellationToken);
		}

		private async Task HandleNextMessageAsync(string messageEnvelopeAsString)
		{
			try
			{
				var payload = JsonConvert.DeserializeObject<MessageBrokerEnvelope<MessageBrokerExternalOrderResponseDTO>>(messageEnvelopeAsString);
				var handler = MessageHandlerFactory.GetHandler(MessageTypes.OrderItemsProcessedByProvider);
				await handler.ExecuteAsync(payload);
			}
			catch (Exception ex)
			{
				throw new Exception("Message processing failed.", ex);
			}
		}
	}
}
