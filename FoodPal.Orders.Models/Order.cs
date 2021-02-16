using FoodPal.Orders.Enums;
using System;
using System.Collections.Generic;

namespace FoodPal.Orders.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DeliveryDetails DeliveryDetails { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItem> Items { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

    }
}
