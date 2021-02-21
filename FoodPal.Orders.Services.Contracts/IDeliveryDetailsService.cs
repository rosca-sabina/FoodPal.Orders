using FoodPal.Orders.DTOs;
using System.Threading.Tasks;

namespace FoodPal.Orders.Services.Contracts
{
    public interface IDeliveryDetailsService
    {
        Task<DeliveryDetailsDTO> GetOrderDeliveryDetailsAsync(int orderId);
    }
}
