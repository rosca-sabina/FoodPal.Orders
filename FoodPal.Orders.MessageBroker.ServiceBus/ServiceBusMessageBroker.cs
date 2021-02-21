using Azure.Messaging.ServiceBus;
using FoodPal.Orders.MessageBroker.Contracts;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FoodPal.Orders.MessageBroker.ServiceBus
{
    public class ServiceBusMessageBroker: IMessageBroker
    {
        private readonly string _messageBrokerEndpoint;
        private MessageReceivedEventHandler _messageHandler;
        private ServiceBusProcessor _serviceBusProcessor;
        private ServiceBusClient _serviceBusMessageReceiverClient;

        public ServiceBusMessageBroker(IOptions<MessageBrokerConnectionSettings> connectionSettings)
        {
            _messageBrokerEndpoint = connectionSettings.Value.Endpoint;
        }

        public void RegisterMessageReceiver(string queueName, MessageReceivedEventHandler messageHandler)
        {
            _messageHandler = messageHandler;
            _serviceBusMessageReceiverClient = new ServiceBusClient(_messageBrokerEndpoint);
            _serviceBusProcessor = _serviceBusMessageReceiverClient.CreateProcessor(
                queueName,
                new ServiceBusProcessorOptions() { 
                    ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete
                }
            );

            _serviceBusProcessor.ProcessMessageAsync += MessageHandlerAsync;
            _serviceBusProcessor.ProcessErrorAsync += ErrorHandlerAsync;
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

        public async Task StartListenerAsync()
        {
            await _serviceBusProcessor.StartProcessingAsync();
        }

        public async Task StopListenerAsync()
        {
            await _serviceBusProcessor.StopProcessingAsync();
            await _serviceBusMessageReceiverClient.DisposeAsync();
        }

        private async Task MessageHandlerAsync(ProcessMessageEventArgs args)
        {
            var messageAsString = args.Message.Body.ToString();
            await _messageHandler(messageAsString);

            await args.CompleteMessageAsync(args.Message);
        }

        private async Task ErrorHandlerAsync(ProcessErrorEventArgs args)
        {
            throw new Exception("Error occured in Service Bus message handler.", args.Exception);
        }
    }
}
