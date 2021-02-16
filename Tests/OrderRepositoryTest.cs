using FoodPal.Orders.Data;
using FoodPal.Orders.Data.Repositories;
using FoodPal.Orders.Enums;
using FoodPal.Orders.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class OrderRepositoryTest
    {
        private readonly OrdersContext _ordersContext;
        private readonly OrderRepository _orderRepository;

        public OrderRepositoryTest()
        {
            DbContextOptions<OrdersContext> ordersContextOptions = new DbContextOptionsBuilder<OrdersContext>()
                .UseInMemoryDatabase("FoodPal.Orders")
                .Options;

            _ordersContext = new OrdersContext(ordersContextOptions);
            _orderRepository = new OrderRepository(_ordersContext);

            List<Order> orders = new List<Order>()
            {
                new Order
                {
                    Id = 1,
                    CustomerId = "101",
                    CustomerName = "Customer 1",
                    CustomerEmail = "customer1@gmail.com",
                    CreatedAt = new DateTime(2021, 2, 1, 20, 12, 0),
                    LastUpdatedAt = new DateTime(2021, 2, 17, 13, 12, 0),
                    Status = OrderStatus.New
                },
                new Order
                {
                    Id = 2,
                    CustomerId = "101",
                    CustomerName = "Customer 1",
                    CustomerEmail = "customer1@gmail.com",
                    CreatedAt = new DateTime(2021, 2, 1, 20, 12, 0),
                    LastUpdatedAt = new DateTime(2021, 2, 17, 20, 12, 0),
                    Status = OrderStatus.InProgress
                },
                new Order
                {
                    Id = 3,
                    CustomerId = "102",
                    CustomerName = "Customer 2",
                    CustomerEmail = "customer2@yahoo.com",
                    CreatedAt = new DateTime(2021, 2, 1, 20, 12, 0),
                    LastUpdatedAt = new DateTime(2021, 2, 19, 20, 12, 0),
                    Status = OrderStatus.New
                },
                new Order
                {
                    Id = 4,
                    CustomerId = "101",
                    CustomerName = "Customer 1",
                    CustomerEmail = "customer1@gmail.com",
                    CreatedAt = new DateTime(2021, 2, 1, 20, 12, 0),
                    LastUpdatedAt = new DateTime(2021, 2, 8, 20, 12, 0),
                    Status = OrderStatus.Delivering
                },
                new Order
                {
                    Id = 5,
                    CustomerId = "101",
                    CustomerName = "Customer 1",
                    CustomerEmail = "customer1@gmail.com",
                    CreatedAt = new DateTime(2021, 2, 1, 20, 12, 0),
                    LastUpdatedAt = new DateTime(2021, 2, 14, 20, 12, 0),
                    Status = OrderStatus.New
                }
            };

            if (_ordersContext.Orders.Count<Order>() == 0)
            {
                _ordersContext.Orders.AddRange(orders);
                _ordersContext.SaveChanges();
            }
        }

        [Fact]
        public async void Test_GetByIdAsync()
        {
            int orderId = 1;
            string expectedCustomerId = "101";
            var order = await _orderRepository.GetByIdAsync(orderId);

            Assert.Equal(orderId, order.Id);
            Assert.Equal(expectedCustomerId, order.CustomerId);
        }

        [Fact]
        public async void Test_GetByIdAsync_NullOrder()
        {
            int orderId = 999;
            var order = await _orderRepository.GetByIdAsync(orderId);

            Assert.Null(order);
        }

        [Fact]
        public async void Test_GetStatusAsync()
        {
            int orderId = 1;
            OrderStatus? orderStatus = await _orderRepository.GetStatusAsync(orderId);

            Assert.Equal(OrderStatus.New, orderStatus);
        }

        [Fact]
        public async void Test_GetStatusAsync_NullOrder()
        {
            int orderId = 999;
            OrderStatus? orderStatus = await _orderRepository.GetStatusAsync(orderId);

            Assert.Null(orderStatus);
        }

        [Fact]
        public async void Test_GetByFiltersAsync()
        {
            string customerId = "101";
            int page = 1;
            int pageSize = 10;

            var orders = await _orderRepository.GetByFiltersAsync(customerId, null, page, pageSize);

            int expectedItemCount = 4;
            Assert.Equal(expectedItemCount, orders.AllOrdersCount);
            Assert.Equal(expectedItemCount, orders.Orders.Count());

            Assert.Collection(
                orders.Orders,
                item => Assert.Equal(2, item.Id),
                item => Assert.Equal(1, item.Id),
                item => Assert.Equal(5, item.Id),
                item => Assert.Equal(4, item.Id)
            );
        }

        [Fact]
        public async void Test_GetByFiltersAsync_Status()
        {
            string customerId = "101";
            int page = 1;
            int pageSize = 2;

            var orders = await _orderRepository.GetByFiltersAsync(customerId, OrderStatus.New, page, pageSize);

            int expectedItemCount = 2;
            Assert.Equal(expectedItemCount, orders.AllOrdersCount);
            Assert.Equal(expectedItemCount, orders.Orders.Count());

            Assert.Collection(
                orders.Orders,
                item => Assert.Equal(1, item.Id),
                item => Assert.Equal(5, item.Id)
            );

            Assert.Collection(
                orders.Orders,
                item => Assert.Equal(OrderStatus.New, item.Status),
                item => Assert.Equal(OrderStatus.New, item.Status)
            );
        }

        [Fact]
        public async void Test_CreateAsync()
        {
            var newOrder = new Order
            {
                Id = 6,
                CustomerId = "101",
                CustomerName = "Customer 1",
                CustomerEmail = "customer1@gmail.com",
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                Status = OrderStatus.New
            };

            var createdOrder = await _orderRepository.CreateAsync(newOrder);

            var orderFromRepository = await _orderRepository.GetByIdAsync(newOrder.Id);

            Assert.Equal(newOrder.Id, createdOrder.Id);
            Assert.NotNull(orderFromRepository);
            Assert.Equal(newOrder.Id, orderFromRepository.Id);
        }

        [Fact]
        public async void Test_UpdateStatusAsync()
        {
            int orderId = 3;
            var order = await _orderRepository.GetByIdAsync(orderId);
            await _orderRepository.UpdateStatusAsync(order, OrderStatus.InProgress);

            order = await _orderRepository.GetByIdAsync(orderId);
            Assert.Equal(OrderStatus.InProgress, order.Status);
        }

        [Fact]
        public async void Test_UpdateStatusAsync_NullOrder()
        {
            var fakeOrder = new Order
            {
                Id = 999,
                CustomerId = "101",
                CustomerName = "Customer 1",
                CustomerEmail = "customer1@gmail.com",
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                Status = OrderStatus.New
            };

            Func<Task> updateFakeOrder = () => _orderRepository.UpdateStatusAsync(fakeOrder, OrderStatus.InProgress);

            await Assert.ThrowsAsync<Exception>(updateFakeOrder);
        }
    }
}
