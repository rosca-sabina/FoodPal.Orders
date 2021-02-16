using FoodPal.Orders.Data.Configurations;
using FoodPal.Orders.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodPal.Orders.Data
{
    public class OrdersContext: DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public OrdersContext(DbContextOptions options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfiguration<Order>(new OrderEntityTypeConfiguration());
            // modelBuilder.ApplyConfiguration<OrderItem>(new OrderItemEntityTypeConfiguration());
            // modelBuilder.ApplyConfiguration<DeliveryDetails>(new DeliveryDetailsEntityTypeConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderEntityTypeConfiguration).Assembly);
        }
    }
}
