using FoodPal.Orders.DTOs;
using System.Threading.Tasks;

namespace FoodPal.Orders.Services.Contracts
{
    public interface IOrderService
    {
        Task<string> CreateAsync(NewOrderDTO newOrder);
    }
}
