using FoodPal.Orders.Contracts;
using FoodPal.Orders.Enums;
using FoodPal.Orders.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodPal.Orders.Data.Repositories
{
    public class OrderItemRepository: IOrderItemRepository
    {
        private readonly OrdersContext _ordersContext;
        public OrderItemRepository(OrdersContext ordersContext)
        {
            _ordersContext = ordersContext;
        }

        public async Task<List<OrderItem>> GetItemsAsync(int orderId)
        {
            try
            {
                var orderItems = await _ordersContext.OrderItems
                    .Where(x => x.OrderId == orderId)
                    .ToListAsync();

                return orderItems;
            }
            catch(Exception ex)
            {
                throw new Exception($"Order items could not be retrieved. Reason: {ex.Message}");
            }
        }

        public async Task<OrderItem> GetOrderItemAsync(int orderId, int orderItemId)
        {
            try
            {
                var orderItem = await _ordersContext.OrderItems.FindAsync(orderItemId);
                return orderItem;
            }
            catch (Exception ex)
            {
                throw new Exception($"Order item could not be retrieved. Reason: {ex.Message}");
            }
        }

        public async Task UpdateStatusAsync(OrderItem orderItemEntity, OrderItemStatus newStatus)
        {
            if (orderItemEntity is null)
            {
                throw new ArgumentNullException(nameof(orderItemEntity));
            }

            try
            {
                var orderItem = await _ordersContext.OrderItems.FindAsync(orderItemEntity.Id);
                if(orderItem == null)
                {
                    throw new Exception("No order item with the specified Id exists.");
                }

                orderItem.Status = newStatus;
                _ordersContext.OrderItems.Update(orderItem);

                await _ordersContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Order item status could not be updated. Reason: {ex.Message}");
            }
        }
    }
}
