namespace SilkRoad.Core;


public record PlaceOrderDTO
{
    public int DeliveryMethodID { get; set; }
    public string BasketID { get; set; } = null!;
}