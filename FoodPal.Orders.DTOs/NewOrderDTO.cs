using System.Collections.Generic;

namespace FoodPal.Orders.DTOs
{
    public class NewOrderDTO
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DeliveryDetailsDTO DeliveryDetails { get; set; }
        public List<NewOrderItemDTO> Items { get; set; }
        public string Comments { get; set; }
    }
}
