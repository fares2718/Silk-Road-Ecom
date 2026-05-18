namespace SilkRoad.Core.Entities;

public partial class Product
{
    public int ProductID { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; } = string.Empty;

    public int CategoryID { get; set; }

    public decimal Price { get; set; }

    public Category Category { get; set; } = null!;
    public ICollection<ProductImage> ProductImages { get; set; } = new HashSet<ProductImage>();
}
