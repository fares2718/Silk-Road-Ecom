using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilkRoad.Core;

namespace SilkRoad.Infrastructure;

public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.HasKey(x => x.DeliveryMethodId);

        builder.Property(x => x.MethodName)
            .HasMaxLength(100);

        builder.Property(x => x.Description)
        .HasMaxLength(255);

        builder.Property(x => x.DeliveryTime)
            .HasMaxLength(100);

        builder.Property(x => x.Price)
            .HasPrecision(18, 2);

    builder.HasOne(x => x.Provider)
        .WithMany(x => x.DeliveryMethods)
        .HasForeignKey(x => x.ProviderId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
