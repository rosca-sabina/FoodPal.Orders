using AutoMapper;
using FoodPal.Orders.BackgroundServices.Handlers.Contracts;
using FoodPal.Orders.BackgroundServices.Settings;
using FoodPal.Orders.Data;
using FoodPal.Orders.DTOs;
using FoodPal.Orders.DTOs.External;
using FoodPal.Orders.Enums;
using FoodPal.Orders.MessageBroker.Contracts;
using FoodPal.Orders.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodPal.Orders.BackgroundServices.Handlers
{
    public class NewOrderMessageHandler : BaseMessageHandler, IMessageHandler
    {
        private readonly IOrdersUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HttpProviderEndpoints _httpProviderEndpoints;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<NewOrderMessageHandler> _logger;

        public NewOrderMessageHandler(IMapper mapper, IOrdersUnitOfWork unitOfWork, IOptions<HttpProviderEndpoints> httpProviderEndpoints, IMessageBroker messageBroker, ILogger<NewOrderMessageHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpProviderEndpoints = httpProviderEndpoints.Value;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public async Task ExecuteAsync<TPayload>(MessageBrokerEnvelope<TPayload> messageEnvelope)
        {
            var payload = messageEnvelope.Data as NewOrderDTO;

            if(payload is null)
            {
                throw new ArgumentException($"{this.GetType().Name} cannot handle payload of type {messageEnvelope.Data.GetType().Name}.");
            }

            var persistedOrder = await SaveNewOrderAsync(payload);

            var grouppedOrderItems = persistedOrder.Items.GroupBy(x => x.ProviderId).ToDictionary(x => x.Key, v => v.ToList());

            foreach(var providerItems in grouppedOrderItems)
            {
                switch (providerItems.Key.ToLower())
                {
                    case "chefsexperience":
                    case "greekcuisine":
                        string endpoint = GetProviderEndpoint(providerItems.Key.ToLower());
                        await SendOrderRequestToProviderViaHttpAsync(endpoint, persistedOrder.Id, providerItems.Value);
                        await UpdateOrderItemsStatus(providerItems.Value);
                        break;
                    case "xyz":
                    case "kfc":
                    case "prestopizza":
                        await SendOrderRequestToProviderViaMessageBrokerAsync(providerItems.Key.ToLower(), persistedOrder.Id, providerItems.Value);
                        await UpdateOrderItemsStatus(providerItems.Value);
                        break;
                    default:
                        throw new Exception($"Handling order items for provider '{providerItems.Key}' is not supported.");
                }
            }

            await UpdateOrderStatus(persistedOrder);
        }

        #region Private methods
        private async Task<Order> SaveNewOrderAsync(NewOrderDTO newOrder)
        {
            var newOrderModel = _mapper.Map<NewOrderDTO, Order>(newOrder);
            newOrderModel.CreatedAt = newOrderModel.LastUpdatedAt = DateTime.Now;
            newOrderModel.Status = OrderStatus.New;

            foreach(var orderItem in newOrderModel.Items)
            {
                orderItem.Status = OrderItemStatus.New;
            }

            return await _unitOfWork.OrderRepository.CreateAsync(newOrderModel);
        }

        private async Task SendOrderRequestToProviderViaHttpAsync(string providerUri, int orderId, List<OrderItem> items)
        {
            var selfCallbackBaseUri = new Uri(_httpProviderEndpoints.SelfCallbackBaseEndpoint);

            var httpPayload = new HttpExternalOrderRequestDTO
            {
                OrderId = orderId,
                OrderItems = items.Select(x =>  new HttpExternalOrderItemRequestDTO { 
                    OrderItemId = x.Id,
                    Name = x.Name,
                    Quantity = x.Quantity,
                    CallbackEndpoint = new Uri(selfCallbackBaseUri, $"OrderItems?orderId={orderId}&orderItemId={x.Id}&propertyName=status&propertyValue={(int)OrderItemStatus.Ready}").ToString()
                }).ToList()
            };

            await new HttpProxy().PostAsync<HttpExternalOrderRequestDTO, string>(providerUri, httpPayload);
        }

        private async Task SendOrderRequestToProviderViaMessageBrokerAsync(string providerId, int orderId, List<OrderItem> orderItems)
        {
            var messageContent = new MessageBrokerExternalOrderRequestDTO
            {
                OrderId = orderId,
                OrderItems = orderItems.Select(x => new MessageBrokerExternalOrderItemRequestDTO { OrderItemId = x.Id, Name = x.Name, Quantity = x.Quantity }).ToList()
            };
            var payload = new MessageBrokerEnvelope<MessageBrokerExternalOrderRequestDTO>(MessageTypes.ProviderNewOrder, messageContent);

            await _messageBroker.SendMessageAsync($"provider-{providerId}-request", payload);
        }

        private async Task UpdateOrderItemsStatus(List<OrderItem> orderItems)
        {
            foreach(var item in orderItems)
            {
                await _unitOfWork.OrderItemRepository.UpdateStatusAsync(item, OrderItemStatus.InProgress);
            }
        }

        private async Task UpdateOrderStatus(Order order)
        {
            await _unitOfWork.OrderRepository.UpdateStatusAsync(order, OrderStatus.InProgress);
        }

        private string GetProviderEndpoint(string provider)
        {
            switch(provider.ToLowerInvariant()){
                case "chefsexperience":
                    return _httpProviderEndpoints.ChefsExperienceEndpoint;
                case "greekcuisine":
                    return _httpProviderEndpoints.GreekCuisineEndpoint;
                default:
                    throw new Exception($"No endpoint exists for provider {provider}.");
            }
        }
        #endregion
    }
}
