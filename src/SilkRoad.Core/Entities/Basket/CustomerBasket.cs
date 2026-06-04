namespace SilkRoad.Core;

public class CustomerBasket
{
    public CustomerBasket() { }

    public CustomerBasket(string basketId)
    {
        BasketID = basketId;
    }

    public string PaymentIntentId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string BasketID { get; set; } = string.Empty;
    public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
}
