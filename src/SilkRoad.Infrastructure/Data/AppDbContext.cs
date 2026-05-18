using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SilkRoad.Core.Entities;

namespace SilkRoad.Infrastructure;

public partial class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductImage> ProductImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        OnModelCreatingPartial(modelBuilder);
    }

    // Can be implemented in another file for additional configuration
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


}
