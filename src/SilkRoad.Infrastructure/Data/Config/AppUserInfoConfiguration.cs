using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilkRoad.Core;
using SilkRoad.Core.Entities;

namespace SilkRoad.Infrastructure;

public class AppUserInfoConfiguration : IEntityTypeConfiguration<AppUserInfo>
{
    public void Configure(EntityTypeBuilder<AppUserInfo> builder)
    {
        builder.Property(aui => aui.Street)
            .HasMaxLength(200);
        
        builder.Property(aui => aui.ZipCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasOne(aui => aui.City)
            .WithMany(c => c.AppUsersInfos)
            .HasForeignKey(aui => aui.CityID);
    }
}