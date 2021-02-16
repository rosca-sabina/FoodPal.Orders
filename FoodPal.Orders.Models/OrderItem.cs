using FoodPal.Orders.Enums;

namespace FoodPal.Orders.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string Name { get; set; }
        public string ProviderId { get; set; }
        public short Quantity { get; set; }
        public decimal Price { get; set; }
        public OrderItemStatus Status { get; set; }
    }
}
