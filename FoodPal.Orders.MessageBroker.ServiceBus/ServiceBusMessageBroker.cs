using Azure.Messaging.ServiceBus;
using FoodPal.Orders.MessageBroker.Contracts;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FoodPal.Orders.MessageBroker.ServiceBus
{
    public class ServiceBusMessageBroker: IMessageBroker
    {
        private readonly string _messageBrokerEndpoint;
        public ServiceBusMessageBroker(IOptions<MessageBrokerConnectionSettings> connectionSettings)
        {
            _messageBrokerEndpoint = connectionSettings.Value.Endpoint;
        }

        public async Task SendMessageAsync<TMessage>(string queueName, TMessage message)
        {
            await using (ServiceBusClient sbClient = new ServiceBusClient(_messageBrokerEndpoint))
            {
                var sender = sbClient.CreateSender(queueName);

                var serializedMessage = JsonConvert.SerializeObject(message);
                var sbMessage = new ServiceBusMessage(serializedMessage);

                await sender.SendMessageAsync(sbMessage);
            }
        }
    }
}
