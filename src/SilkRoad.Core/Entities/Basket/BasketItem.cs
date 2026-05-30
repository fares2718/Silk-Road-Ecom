namespace SilkRoad.Core;

public class BasketItem
{
    public int ItemID { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ImageURL { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }

}
