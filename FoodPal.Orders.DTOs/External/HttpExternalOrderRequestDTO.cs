using System.Collections.Generic;

namespace FoodPal.Orders.DTOs.External
{
    public class HttpExternalOrderRequestDTO
    {
        public int OrderId { get; set; }
        public List<HttpExternalOrderItemRequestDTO> OrderItems { get; set; }
    }
}
