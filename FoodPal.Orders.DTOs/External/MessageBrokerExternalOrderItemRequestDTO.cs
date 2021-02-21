namespace FoodPal.Orders.DTOs.External
{
    public class MessageBrokerExternalOrderItemRequestDTO
    {
        public int OrderItemId { get; set; }
        public string Name { get; set; }
        public short Quantity { get; set; }
    }
}
