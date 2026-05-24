using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilkRoad.Core.Entities;

namespace SilkRoad.Infrastructure;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.ProductName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(255);

        builder.Property(p => p.NewPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.OldPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryID)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(p => p.ProductName)
            .IsUnique();
            
        builder.HasIndex(p => p.CategoryID);
    }

}
