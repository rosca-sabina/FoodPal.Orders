using System.Collections.Generic;

namespace FoodPal.Orders.DTOs.External
{
    public class MessageBrokerExternalOrderRequestDTO
    {
        public int OrderId { get; set; }
        public List<MessageBrokerExternalOrderItemRequestDTO> OrderItems { get; set; }
    }
}
