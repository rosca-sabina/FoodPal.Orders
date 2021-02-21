using System.Collections.Generic;

namespace FoodPal.Orders.DTOs.External
{
    public class MessageBrokerExternalOrderResponseDTO
    {
        public int OrderId { get; set; }
        public List<int> OrderItemIds { get; set; }
    }
}
