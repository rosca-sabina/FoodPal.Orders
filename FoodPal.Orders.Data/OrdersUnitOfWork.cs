using FoodPal.Orders.Contracts;
using FoodPal.Orders.Data.Repositories;
using System;

namespace FoodPal.Orders.Data
{
    public class OrdersUnitOfWork : IOrdersUnitOfWork
    {
        private readonly Lazy<IOrderRepository> _orderRepository;
        private readonly Lazy<IOrderItemRepository> _orderItemRepository;

        public OrdersUnitOfWork(OrdersContext dbContext)
        {
            _orderRepository = new Lazy<IOrderRepository>(new OrderRepository(dbContext));
            _orderItemRepository = new Lazy<IOrderItemRepository>(new OrderItemRepository(dbContext));
        }

        public IOrderRepository OrderRepository => _orderRepository.Value;
        public IOrderItemRepository OrderItemRepository => _orderItemRepository.Value;
    }
}
