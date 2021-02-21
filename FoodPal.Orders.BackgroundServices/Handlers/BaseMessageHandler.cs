using FoodPal.Orders.MessageBroker.Contracts;
using System;

namespace FoodPal.Orders.BackgroundServices.Handlers
{
    public abstract class BaseMessageHandler
	{
		protected TPayload GetEnvelopePayload<TMessage, TPayload>(MessageBrokerEnvelope<TMessage> messageEnvelope)
		{
			if (messageEnvelope.Data is not TPayload payload)
			{
				throw new ArgumentException($"'{this.GetType().Name}' cannot handle payloads of type {messageEnvelope.Data.GetType().Name}");
			}

			return payload;
		}
	}
}
