namespace SilkRoad.Core;

public class Order
{
    public Guid OrderId { get; set; }

    public string CustomerId { get; set; } = null!;

    // Shipping snapshot
    public ShippingAddressSnapshot ShippingAddressSnapshot { get; set; } = null!;

    // Delivery snapshot
    public DeliverySnapshot DeliverySnapshot { get; set; } = null!;

    public decimal SubTotal { get; set; }

    public decimal Total { get; set; } 

    public enStatus OrderStatus { get; set; } = enStatus.Pending;

    public DateTime OrderDate { get; set; }

    public AppUser Customer { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();
}
