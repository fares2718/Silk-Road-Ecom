namespace SilkRoad.Core.Entities;

public partial class ProductImage
{
    public int ImageID { get; set; }

    public int ProductID { get; set; }

    public string ImageURL { get; set; } = null!;

    public Product Product { get; set; } = null!;
}
