namespace FoodPal.Orders.DTOs
{
    public class NewOrderItemDTO
    {
        public string Name { get; set; }
        public string ProviderId { get; set; }
        public short Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
