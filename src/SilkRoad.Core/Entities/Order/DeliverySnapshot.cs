namespace SilkRoad.Core;

public class DeliverySnapshot
{
    public string DeliveryProviderName { get; set; } = null!;
    public string DeliveryMethodName { get; set; } = null!;
    public decimal DeliveryPrice { get; set; }
}
