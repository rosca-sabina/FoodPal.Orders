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
    public class OrderItemRepositoryTest
    {
        private readonly OrdersContext _ordersContext;
        private readonly OrderItemRepository _orderItemRepository;

        public OrderItemRepositoryTest()
        {
            DbContextOptions<OrdersContext> ordersContextOptions = new DbContextOptionsBuilder<OrdersContext>()
                .UseInMemoryDatabase("FoodPal.Orders")
                .Options;

            _ordersContext = new OrdersContext(ordersContextOptions);
            _orderItemRepository = new OrderItemRepository(_ordersContext);

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
                }
            };

            if (_ordersContext.Orders.Count<Order>() == 0)
            {
                _ordersContext.Orders.AddRange(orders);
            }

            List<OrderItem> orderItems = new List<OrderItem>()
            {
                new OrderItem
                {
                    Id = 1,
                    OrderId = 1,
                    ProviderId = "Prov1",
                    Price = (decimal)12.35,
                    Quantity = 1,
                    Name = "Item 1",
                    Status = OrderItemStatus.New
                },
                new OrderItem
                {
                    Id = 2,
                    OrderId = 1,
                    ProviderId = "Prov1",
                    Price = (decimal)5.25,
                    Quantity = 2,
                    Name = "Item 2",
                    Status = OrderItemStatus.New
                },
                new OrderItem
                {
                    Id = 3,
                    OrderId = 1,
                    ProviderId = "Prov2",
                    Price = 10,
                    Quantity = 1,
                    Name = "Item 3",
                    Status = OrderItemStatus.InProgress
                }
            };

            if (_ordersContext.OrderItems.Count<OrderItem>() == 0)
            {
                _ordersContext.OrderItems.AddRange(orderItems);
            }

            _ordersContext.SaveChanges();
        }

        [Fact]
        public async void Test_GetItemsAsync()
        {
            int orderId = 1;
            var orderItems = await _orderItemRepository.GetItemsAsync(orderId);

            Assert.Equal(3, orderItems.Count);
        }

        [Fact]
        public async void Test_GetItemsAsync_NullOrder()
        {
            int orderId = 999;
            var orderItems = await _orderItemRepository.GetItemsAsync(orderId);

            Assert.Empty(orderItems);
        }

        [Fact]
        public async void Test_GetOrderItemAsync()
        {
            int orderItemId = 3;
            int orderId = 1;
            var orderItem = await _orderItemRepository.GetOrderItemAsync(orderId, orderItemId);

            string expectedName = "Item 3";

            Assert.Equal(expectedName, orderItem.Name);
            Assert.Equal(orderId, orderItem.OrderId);
        }

        [Fact]
        public async void Test_GetOrderItemAsync_NullOrderItem()
        {
            int orderItemId = 999;
            int orderId = 1;
            var orderItem = await _orderItemRepository.GetOrderItemAsync(orderId, orderItemId);

            Assert.Null(orderItem);
        }

        [Fact]
        public async void Test_UpdateStatusAsync()
        {
            int orderItemId = 2;
            var order = await _orderItemRepository.GetOrderItemAsync(1, orderItemId);

            await _orderItemRepository.UpdateStatusAsync(order, OrderItemStatus.Ready);

            Assert.Equal(OrderItemStatus.Ready, order.Status);
        }

        [Fact]
        public async void Test_UpdateStatusAsync_FakeOrderItem()
        {
            OrderItem fakeOrderItem = new OrderItem
            {
                Id = 999,
                OrderId = 1,
                ProviderId = "Prov999",
                Price = (decimal)1.23,
                Quantity = 1,
                Name = "Fake Item",
                Status = OrderItemStatus.New
            };

            Func<Task> updateFakeOrderItem = () =>  _orderItemRepository.UpdateStatusAsync(fakeOrderItem, OrderItemStatus.Ready);

            await Assert.ThrowsAsync<Exception>(updateFakeOrderItem);
        }
    }
}
