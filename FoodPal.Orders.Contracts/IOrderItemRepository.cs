using FoodPal.Orders.Enums;
using FoodPal.Orders.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodPal.Orders.Data.Contracts
{
    public interface IOrderItemRepository
    {
		/// <summary>
		/// Returns an order item by order id and order item id.
		/// </summary>
		/// <param name="orderId">The order id.</param>
		/// <param name="orderItemId">The order item id.</param>
		/// <returns>The order item.</returns>
		Task<OrderItem> GetOrderItemAsync(int orderId, int orderItemId);

		/// <summary>
		/// Returns the list of order items contained in one order.
		/// </summary>
		/// <param name="orderId">The order id.</param>
		/// <returns>A list of order items.</returns>
		Task<List<OrderItem>> GetItemsAsync(int orderId);

		/// <summary>
		/// Updates the order item status.
		/// </summary>
		/// <param name="orderItemEntity">The order item.</param>
		/// <param name="newStatus">The new status.</param>
		/// <returns></returns>
		Task UpdateStatusAsync(OrderItem orderItemEntity, OrderItemStatus newStatus);
	}
}
