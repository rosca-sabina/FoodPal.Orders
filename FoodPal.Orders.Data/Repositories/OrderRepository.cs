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
    public class OrderRepository: IOrderRepository
    {
        private readonly OrdersContext _ordersContext;
        public OrderRepository(OrdersContext ordersContext)
        {
            _ordersContext = ordersContext;
        }

        public async Task<Order> CreateAsync(Order newOrder)
        {
            if(newOrder is null)
            {
                throw new ArgumentNullException(nameof(newOrder));
            }

            try
            {
                await _ordersContext.Orders.AddAsync(newOrder);
                await _ordersContext.SaveChangesAsync();

                return newOrder;
            }
            catch (Exception ex)
            {
                throw new Exception($"Order could not be saved. Reason: {ex.Message}.");
            }
        }

        public async Task<(IEnumerable<Order> Orders, int AllOrdersCount)> GetByFiltersAsync(string customerId, OrderStatus? status, int page, int pageSize)
        {
            if (customerId is null)
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            try
            {
                var orders = await _ordersContext.Orders
                    .Where(x => x.CustomerId.Equals(customerId)
                                && (status == null || x.Status == status))
                    .OrderByDescending(x => x.LastUpdatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (orders, orders.Count);
            }
            catch(Exception ex)
            {
                throw new Exception($"Orders could not be retrieved. Reason: {ex.Message}");
            }
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            try
            {
                var order = await _ordersContext.Orders.FindAsync(orderId);
                return order;
            }
            catch(Exception ex)
            {
                throw new Exception($"Order could not be retrieved. Reason: {ex.Message}.");
            }
        }

        public async Task<OrderStatus?> GetStatusAsync(int orderId)
        {
            try
            {
                var order = await _ordersContext.Orders.FindAsync(orderId);
                if(order == null)
                {
                    return null;
                }

                return order.Status;
            }
            catch (Exception ex)
            {
                throw new Exception($"Order status could not be retrieved. Reason: {ex.Message}.");
            }
        }

        public async Task UpdateStatusAsync(Order orderEntity, OrderStatus newStatus)
        {
            try
            {
                var order = await _ordersContext.Orders.FindAsync(orderEntity.Id);
                if (order == null)
                {
                    throw new Exception("No order with the specified Id exists.");
                }

                order.Status = newStatus;
                _ordersContext.Orders.Update(order);

                await _ordersContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception($"Order status could not be updated. Reason: {ex.Message}.");
            }
        }
    }
}
