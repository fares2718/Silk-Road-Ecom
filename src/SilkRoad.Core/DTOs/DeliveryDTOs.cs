namespace SilkRoad.Core;

public record DeliveryMethodDTO
{
    public int DeliveryMethodId { get; set; }

    public string ProviderName { get; set; } = null!;

    public string MethodName { get; set; } = null!;

    public string? Description { get; set; }

    public string DeliveryTime { get; set; } = null!;

    public decimal Price { get; set; }

    public bool Available { get; set; } = true;

}
