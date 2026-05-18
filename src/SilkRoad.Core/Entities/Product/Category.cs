namespace SilkRoad.Core.Entities;

public partial class Category
{
    public int CategoryID { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? CategoryDescription { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
