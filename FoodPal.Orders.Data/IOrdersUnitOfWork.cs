using FoodPal.Orders.Contracts;

namespace FoodPal.Orders.Data
{
    public interface IOrdersUnitOfWork
    {
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
    }
}
