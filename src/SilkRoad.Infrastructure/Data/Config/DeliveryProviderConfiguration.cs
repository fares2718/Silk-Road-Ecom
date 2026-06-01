using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilkRoad.Core;

namespace SilkRoad.Infrastructure;

public class DeliveryProviderConfiguration : IEntityTypeConfiguration<DeliveryProvider>
{
    public void Configure(EntityTypeBuilder<DeliveryProvider> builder)
    {
        builder.HasKey(x => x.ProviderId);

        builder.Property(x => x.ProviderName)
            .HasMaxLength(255)
                .IsRequired();

        builder.HasIndex(x => x.ProviderName)
            .IsUnique();

        builder.Property(x => x.Available)
            .HasDefaultValue(true);
    }
}
