using FoodPal.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodPal.Orders.Data.Configurations
{
    internal class DeliveryDetailsEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryDetails>
    {
        public void Configure(EntityTypeBuilder<DeliveryDetails> builder)
        {
            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.ZipCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.Country)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(12);
        }
    }
}
