using FoodPal.Orders.Enums;
using FoodPal.Orders.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodPal.Orders.Data.Contracts
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="newOrder">New order object model.</param>
        /// <returns>The persisted order.</returns>
        Task<Order> CreateAsync(Order newOrder);

		/// <summary>
		/// Returns an order by id.
		/// </summary>
		/// <param name="orderId">The order id.</param>
		/// <returns>The order result.</returns>
		/// <remarks>If no order is found, null is returned.</remarks>
		Task<Order> GetByIdAsync(int orderId);

		/// <summary>
		/// Returns all orders by customer id and order status (if specified)
		/// </summary>
		/// <param name="customerId">The customer id.</param>
		/// <param name="status">The order status, optional.</param>
		/// <param name="page">Result page.</param>
		/// <param name="pageSize">Result page size.</param>
		/// <returns>The requested page containing the orders specified by filters (customer id and order status).</returns>
		Task<(IEnumerable<Order> Orders, int AllOrdersCount)> GetByFiltersAsync(string customerId, OrderStatus? status, int page, int pageSize);

		/// <summary>
		/// Returns order status.
		/// </summary>
		/// <param name="orderId">The order id.</param>
		/// <returns>The order status.</returns>
		/// <remarks>If no order is found, null is returned.</remarks>
		Task<OrderStatus?> GetStatusAsync(int orderId);

		/// <summary>
		/// Updates the order status
		/// </summary>
		/// <param name="orderEntity">The order entity.</param>
		/// <param name="newStatus">The new status.</param>
		/// <returns></returns>
		Task UpdateStatusAsync(Order orderEntity, OrderStatus newStatus);
	}
}
