using FoodPal.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FoodPal.Orders.Data.Configurations
{
    internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.CustomerId)
                .IsRequired();

            builder.Property(x => x.CustomerName)
                .IsRequired();

            builder.Property(x => x.CustomerEmail)
                .IsRequired();

            builder.Property(x => x.Comments)
                .HasMaxLength(200);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.LastUpdatedAt)
                .IsRequired();
        }
    }
}
