namespace FoodPal.Orders.DTOs
{
    public class OrderItemDTO
    {
        public string Name { get; set; }
        public string ProviderId { get; set; }
        public short Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price;
        public StatusDTO OrderItemStatus { get; set; }
    }
}
