using System;

namespace FoodPal.Orders.MessageBroker.Contracts
{
    public class MessageBrokerEnvelope<TPayload>
    {
        public string RequestId { get; set; }
        public string MesageType { get; set; }
        public TPayload Data { get; set; }

        public MessageBrokerEnvelope(string messageType, TPayload payload, string requestId)
        {
            MesageType = messageType;
            Data = payload;
            RequestId = requestId;
        }

        public MessageBrokerEnvelope(string messageType, TPayload payload): this(messageType, payload, Guid.NewGuid().ToString())
        {

        }
    }
}
