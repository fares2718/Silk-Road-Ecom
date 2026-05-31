using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilkRoad.Core;

namespace SilkRoad.Infrastructure;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.Property(c => c.CountryName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(c => c.CountryName)
            .IsUnique();

        builder.Property(c => c.CountryCode)
            .IsRequired()
            .HasMaxLength(10);
            
        builder.HasIndex(c => c.CountryCode)
            .IsUnique();
    }
}
