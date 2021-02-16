using FoodPal.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodPal.Orders.Data.Configurations
{
    internal class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.ProviderId)
                .IsRequired();


        }
    }
}
