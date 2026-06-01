namespace SilkRoad.Core;

public class Order
{
    public Order()
    {
        Total = SubTotal + DeliveryPrice;
    }

    public Guid OrderId { get; set; }

    public string CustomerId { get; set; } = null!;

    // Shipping snapshot
    public string ShippingFullName { get; set; } = null!;
    public string ShippingStreet { get; set; } = null!;
    public string ShippingCity { get; set; } = null!;
    public string ShippingPostalCode { get; set; } = null!;
    public string ShippingCountry { get; set; } = null!;

    // Delivery snapshot
    public string DeliveryProviderName { get; set; } = null!;
    public string DeliveryMethodName { get; set; } = null!;
    public decimal DeliveryPrice { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Total { get; private set; }

    public DateTime OrderDate { get; set; }

    public AppUser Customer { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();
}
