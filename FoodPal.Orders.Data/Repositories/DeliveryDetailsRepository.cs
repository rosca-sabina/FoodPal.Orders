using FoodPal.Orders.Data.Contracts;
using FoodPal.Orders.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FoodPal.Orders.Data.Repositories
{
    public class DeliveryDetailsRepository : IDeliveryDetailsRepository
    {
        private readonly OrdersContext _ordersContext;

        public DeliveryDetailsRepository(OrdersContext ordersContext)
        {
            _ordersContext = ordersContext;
        }

        public async Task<DeliveryDetails> GetOrderDeliveryDetailsAsync(int orderId)
        {
            try
            {
                var order = await _ordersContext.Orders
                    .Include(o => o.DeliveryDetails)
                    .SingleOrDefaultAsync(o => o.Id == orderId);
                if (order is null)
                {
                    return null;
                }

                return order.DeliveryDetails;
            }
            catch(Exception ex)
            {
                throw new Exception($"Order details could not be retrieved. Reason: {ex.Message}.", ex);
            }
        }
    }
}
