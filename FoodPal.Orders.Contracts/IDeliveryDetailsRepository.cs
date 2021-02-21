using FoodPal.Orders.Models;
using System.Threading.Tasks;

namespace FoodPal.Orders.Data.Contracts
{
    public interface IDeliveryDetailsRepository
    {
        Task<DeliveryDetails> GetOrderDeliveryDetailsAsync(int orderId);
    }
}
