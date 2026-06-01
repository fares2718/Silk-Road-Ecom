namespace SilkRoad.Core;


public class DeliveryProvider
{
    public Guid ProviderId { get; set; }

    public string ProviderName { get; set; } = null!;

    public bool Available { get; set; } = true;

    public ICollection<DeliveryMethod> DeliveryMethods { get; set; }
        = new List<DeliveryMethod>();
}