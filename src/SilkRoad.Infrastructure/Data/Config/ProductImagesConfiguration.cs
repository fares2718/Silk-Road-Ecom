using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilkRoad.Core.Entities;

namespace SilkRoad.Infrastructure;

public class ProductImagesConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.Property(pi => pi.ImageURL)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(pi => pi.Product)
            .WithMany(p => p.ProductImages)
            .HasForeignKey(pi => pi.ProductID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pi => pi.ProductID);
        builder.HasIndex(pi => pi.ImageURL)
        .IsUnique();
    }

}
