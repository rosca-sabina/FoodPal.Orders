using FoodPal.Orders.DTOs;
using FoodPal.Orders.Enums;
using System.Threading.Tasks;

namespace FoodPal.Orders.Services.Contracts
{
    public interface IOrderService
    {
        Task<string> CreateAsync(NewOrderDTO newOrder);
        Task<OrderDTO> GetByIdAsync(int orderId);
        Task<StatusDTO> GetStatusAsync(int orderId);
        Task<PagedResultSetDTO<OrderDTO>> GetByFiltersAsync(string customerId, OrderStatus? status, int page, int pageSize);
        Task PatchOrderAsync(int orderId, GenericPatchDTO orderPatch);

    }
}
