using FoodPal.Orders.Data.Contracts;

namespace FoodPal.Orders.Data
{
    public interface IOrdersUnitOfWork
    {
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
        IDeliveryDetailsRepository DeliveryDetailsRepository { get; }
    }
}
