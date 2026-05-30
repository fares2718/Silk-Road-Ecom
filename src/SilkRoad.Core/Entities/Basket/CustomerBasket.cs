namespace SilkRoad.Core;

public class CustomerBasket
{
    public CustomerBasket() { }

    public CustomerBasket(int basketId)
    {
        BasketId = basketId;
    }
    public int BasketId { get; set; }
    public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
}
