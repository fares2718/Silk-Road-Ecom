namespace SilkRoad.Core;

public class CustomerBasket
{
    public CustomerBasket() { }

    public CustomerBasket(string basketId)
    {
        BasketID = basketId;
    }
    public string BasketID { get; set; } = string.Empty;
    public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
}
