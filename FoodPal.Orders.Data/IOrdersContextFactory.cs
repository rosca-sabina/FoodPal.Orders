namespace FoodPal.Orders.Data
{
    public interface IOrdersContextFactory
    {
        OrdersContext CreateDbContext(string connectionString);
    }
}
