using FoodPal.Orders.Data.Contracts;
using FoodPal.Orders.Data.Repositories;
using System;

namespace FoodPal.Orders.Data
{
    public class OrdersUnitOfWork : IOrdersUnitOfWork
    {
        private readonly Lazy<IOrderRepository> _orderRepository;
        private readonly Lazy<IOrderItemRepository> _orderItemRepository;
        private readonly Lazy<IDeliveryDetailsRepository> _deliveryDetailsRepository;

        public OrdersUnitOfWork(OrdersContext dbContext)
        {
            _orderRepository = new Lazy<IOrderRepository>(new OrderRepository(dbContext));
            _orderItemRepository = new Lazy<IOrderItemRepository>(new OrderItemRepository(dbContext));
            _deliveryDetailsRepository = new Lazy<IDeliveryDetailsRepository>(new DeliveryDetailsRepository(dbContext));
        }

        public IOrderRepository OrderRepository => _orderRepository.Value;
        public IOrderItemRepository OrderItemRepository => _orderItemRepository.Value;
        public IDeliveryDetailsRepository DeliveryDetailsRepository => _deliveryDetailsRepository.Value;
    }
}
