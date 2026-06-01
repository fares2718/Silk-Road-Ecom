namespace SilkRoad.Core;

public class DeliveryMethod
{
    public int DeliveryMethodId { get; set; }

    public Guid ProviderId { get; set; }

    public string MethodName { get; set; } = null!;

    public string? Description { get; set; }

    public string DeliveryTime { get; set; } = null!;

    public decimal Price { get; set; }

    public bool Available { get; set; } = true;

    public DeliveryProvider Provider { get; set; } = null!;
}
