using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SilkRoad.Core;

namespace SilkRoad.Infrastructure;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
                builder.Property(au => au.UserName)
                        .IsRequired()
                        .HasMaxLength(50);

                builder.HasIndex(au => au.UserName)
                        .IsUnique();

                builder.Property(au => au.FirstName)
                        .IsRequired()
                        .HasMaxLength(50);

                builder.Property(au => au.MiddleName)
                        .HasMaxLength(50);

                builder.Property(au => au.LastName)
                        .IsRequired()
                        .HasMaxLength(50);

                builder.HasOne(u => u.AppUserInfo)
                        .WithOne(ui => ui.AppUser)
                        .HasForeignKey<AppUserInfo>(ui => ui.AppUserID);
        }
}


