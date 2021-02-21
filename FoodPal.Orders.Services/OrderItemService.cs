using FoodPal.Orders.Data;
using FoodPal.Orders.DTOs;
using FoodPal.Orders.Enums;
using FoodPal.Orders.Exceptions;
using FoodPal.Orders.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace FoodPal.Orders.Services
{
    public class OrderItemService : BaseService, IOrderItemsService
    {
        private readonly IOrdersUnitOfWork _unitOfWork;
        public OrderItemService(IOrdersUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task PatchOrderItemAsync(int orderId, int orderItemId, GenericPatchDTO orderItemPatch)
        {
            ParameterChecks(new (Func<bool>, Exception)[]
            {
                ( () => orderId > 0, new ArgumentOutOfRangeException(nameof(orderId), $"{nameof(orderId)} must be positive.")),
                ( () => orderItemId > 0, new ArgumentOutOfRangeException(nameof(orderItemId), $"{nameof(orderItemId)} must be positive."))
            });

            var orderItem = await _unitOfWork.OrderItemRepository.GetOrderItemAsync(orderId, orderItemId);
            if(orderItem == null)
            {
                throw new FoodPalNotFoundException(new ErrorInfoDTO
                { 
                    InfoType = ErrorInfoType.Error,
                    Message = $"No order item with id {orderItemId} exists for order {orderId}."
                });
            }

            switch (orderItemPatch.PropertyName.ToLowerInvariant())
            {
                case "status":
                    OrderItemStatus orderItemStatus = ParseOrderItemStatus(orderItemPatch.PropertyValue);
                    await _unitOfWork.OrderItemRepository.UpdateStatusAsync(orderItem, orderItemStatus);
                    break;
                default:
                    throw new Exception($"Patch is not supported for property name '{orderItemPatch.PropertyName}'.");
            }
        }

        #region Private methods
        private OrderItemStatus ParseOrderItemStatus(string orderItemStatus)
        {
            if (Enum.TryParse<OrderItemStatus>(orderItemStatus, true, out var newOrderItemStatus))
            {
                return newOrderItemStatus;
            }

            throw new Exception($"Cannot parse order item status '{orderItemStatus}'.");
        }
        #endregion
    }
}
