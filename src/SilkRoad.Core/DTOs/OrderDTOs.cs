namespace SilkRoad.Core;


public record OrderDTO
{
    public Guid OrderId { get; set; }


    // Shipping snapshot
    public ShippingAddressSnapshot ShippingAddressSnapshot { get; set; } = null!;

    // Delivery snapshot
    public DeliverySnapshot DeliverySnapshot { get; set; } = null!;

    public decimal SubTotal { get; set; }

    public decimal Total { get; set; }

    public enStatus OrderStatus { get; set; } = enStatus.Pending;

    public DateTime OrderDate { get; set; }
    public IReadOnlyList<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
}

public record OrderItemDTO
{
    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; set; }
}

public record PlaceOrderDTO
{
    public int DeliveryMethodID { get; set; }
    public string BasketID { get; set; } = null!;
}