using FluentValidation;
using FoodPal.Orders.DTOs;
using FoodPal.Orders.Enums;
using FoodPal.Orders.MessageBroker.Contracts;
using FoodPal.Orders.Services.Contracts;
using System;
using System.Threading.Tasks;
using FoodPal.Orders.Data;
using AutoMapper;
using System.Linq;
using FoodPal.Orders.Exceptions;
using FoodPal.Orders.Models;
using System.Collections.Generic;

namespace FoodPal.Orders.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IValidator<NewOrderDTO> _newOrderValidator;
        private readonly IOrdersUnitOfWork _ordersUnitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IMessageBroker messageBroker, IValidator<NewOrderDTO> newOrderValidator, IOrdersUnitOfWork ordersUnitOfWork, IMapper mapper)
        {
            _messageBroker = messageBroker;
            _newOrderValidator = newOrderValidator;
            _ordersUnitOfWork = ordersUnitOfWork;
            _mapper = mapper;
        }

        public async Task<string> CreateAsync(NewOrderDTO newOrder)
        {
            ValidateNewOrder(newOrder);

            var payload = new MessageBrokerEnvelope<NewOrderDTO>(MessageTypes.NewOrder, newOrder);
            await _messageBroker.SendMessageAsync("new-orders", payload);

            return payload.RequestId;
        }

        public async Task<PagedResultSetDTO<OrderDTO>> GetByFiltersAsync(string customerId, OrderStatus? status, int page, int pageSize)
        {
            ParameterChecks(new (Func<bool>, Exception)[]
            {
                ( () => page > 0, new ArgumentOutOfRangeException(nameof(page), $"{nameof(page)} must be positive")),
                ( () => pageSize > 0, new ArgumentOutOfRangeException(nameof(pageSize), $"{nameof(pageSize)} must be positive"))
            });

            var result = await _ordersUnitOfWork.OrderRepository.GetByFiltersAsync(customerId, status, page, pageSize);

            if (result.AllOrdersCount == 0)
            {
                return new PagedResultSetDTO<OrderDTO>
                {
                    Data = new List<OrderDTO>(),
                    PaginationInfo = new PaginationInfoDTO { Page = 1, PageSize = pageSize, Total = 0 }
                };
            }

            return new PagedResultSetDTO<OrderDTO>
            {
                Data = _mapper.Map<List<OrderDTO>>(result.Orders),
                PaginationInfo = new PaginationInfoDTO
                {
                    Page = page,
                    PageSize = pageSize,
                    Total = result.AllOrdersCount
                }
            };
        }

        public async Task<OrderDTO> GetByIdAsync(int orderId)
        {
            ParameterChecks(new (Func<bool>, Exception)[]
            {
                ( () => orderId > 0, new ArgumentOutOfRangeException(nameof(orderId), $"{nameof(orderId)} must be positive")),
            });

            var order =  await _ordersUnitOfWork.OrderRepository.GetByIdAsync(orderId);
            if(order == null)
            {
                throw new FoodPalNotFoundException(orderId.ToString());
            }

            return _mapper.Map<Order, OrderDTO>(order);
        }

        public async Task<StatusDTO> GetStatusAsync(int orderId)
        {
            ParameterChecks(new (Func<bool>, Exception)[]
            {
                ( () => orderId > 0, new ArgumentOutOfRangeException(nameof(orderId), $"{nameof(orderId)} must be positive")),
            });

            var order = await _ordersUnitOfWork.OrderRepository.GetByIdAsync(orderId);
            if(order == null)
            {
                throw new FoodPalNotFoundException(orderId.ToString());
            }

            return _mapper.Map<OrderStatus, StatusDTO>(order.Status);
        }

        public async Task PatchOrderAsync(int orderId, GenericPatchDTO orderPatch)
        {
            ParameterChecks(new (Func<bool>, Exception)[]
            {
                ( () => orderId > 0, new ArgumentOutOfRangeException(nameof(orderId), $"{nameof(orderId)} must be positive")),
            });

            var order = await _ordersUnitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new FoodPalNotFoundException(orderId.ToString());
            }

            switch (orderPatch.PropertyName.ToLower())
            {
                case "status":
                    await _ordersUnitOfWork.OrderRepository.UpdateStatusAsync(order, ParseOrderStatus(orderPatch.PropertyValue.ToString()));
                    break;
                default:
                    throw new Exception($"Patch is not supported for property name '{orderPatch.PropertyName}'.");
            }
        }

        #region Private methods
        private OrderStatus ParseOrderStatus(string orderStatus)
        {
            if (Enum.TryParse<OrderStatus>(orderStatus, true, out var newOrderStatus))
            {
                return newOrderStatus;
            }

            throw new Exception($"Cannot parse order status '{orderStatus}'.");
        }

        private void ValidateNewOrder(NewOrderDTO newOrder)
        {
            var failures = _newOrderValidator
                .Validate(newOrder).Errors
                .Where(error => error != null)
                .Select(error => error.ToString())
                .ToList();

            if (failures.Any())
            {
                throw new FoodPalBadParamsException(failures);
            }
        }
        #endregion
    }
}
