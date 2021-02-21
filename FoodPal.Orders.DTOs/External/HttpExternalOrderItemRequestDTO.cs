namespace FoodPal.Orders.DTOs.External
{
    public class HttpExternalOrderItemRequestDTO
    {
        public int OrderItemId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string CallbackEndpoint { get; set; }

    }
}
