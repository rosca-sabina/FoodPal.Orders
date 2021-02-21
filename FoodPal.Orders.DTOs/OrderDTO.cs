using System;
using System.Collections.Generic;

namespace FoodPal.Orders.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DeliveryDetailsDTO DeliveryDetails { get; set; }
        public StatusDTO OrderStatus { get; set; }
        public IEnumerable<OrderItemDTO> Items { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
