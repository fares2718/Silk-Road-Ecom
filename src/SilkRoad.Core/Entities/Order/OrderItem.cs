using SilkRoad.Core.Entities;

namespace SilkRoad.Core;

public class OrderItem
{
    public int OrderItemId { get; set; }

    public Guid OrderId { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; private set; }

    public Order Order { get; set; } = null!;

    public Product Product { get; set; } = null!;
}
