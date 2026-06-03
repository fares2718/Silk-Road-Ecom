using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilkRoad.Core;

namespace SilkRoad.Infrastructure;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.OrderId);

        builder.OwnsOne(x => x.ShippingAddressSnapshot, sa =>
        {
            sa.Property(x => x.ShippingFullName)
                .HasColumnName("ShippingFullName")
                .IsRequired();

            sa.Property(x => x.ShippingStreet)
                .HasColumnName("ShippingStreet")
                .IsRequired();

            sa.Property(x => x.ShippingCity)
                .HasColumnName("ShippingCity")
                .IsRequired();

            sa.Property(x => x.ShippingPostalCode)
                .HasColumnName("ShippingPostalCode")
                .IsRequired();

            sa.Property(x => x.ShippingCountry)
                .HasColumnName("ShippingCountry")
                .IsRequired();
        });

        builder.Property(x => x.SubTotal)
            .HasPrecision(18, 2);

        builder.OwnsOne(x => x.DeliverySnapshot, ds =>
         {
             ds.Property(x => x.DeliveryProviderName)
                 .HasColumnName("DeliveryProviderName")
                 .IsRequired();

             ds.Property(x => x.DeliveryMethodName)
                 .HasColumnName("DeliveryMethodName")
                 .IsRequired();

             ds.Property(x => x.DeliveryPrice)
                 .HasColumnName("DeliveryPrice")
                 .HasPrecision(18, 2);
         });

        builder.Property(x => x.Total)
            .HasPrecision(18, 2)
            .HasComputedColumnSql(
                "[SubTotal] + [DeliveryPrice]",
                stored: true);

        builder.Property(x => x.OrderDate)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(x => x.OrderStatus)
            .HasConversion(o => o.ToString(),
                o => (enStatus)Enum.Parse(typeof(enStatus), o));

        builder.HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
