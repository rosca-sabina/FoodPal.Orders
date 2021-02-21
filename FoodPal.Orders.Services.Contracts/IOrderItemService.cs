using FoodPal.Orders.DTOs;
using System.Threading.Tasks;

namespace FoodPal.Orders.Services.Contracts
{
    public interface IOrderItemsService
	{
		Task PatchOrderItemAsync(int orderId, int orderItemId, GenericPatchDTO orderItemPatch);
	}
}
