namespace SilkRoad.Core;

public class ShippingAddress
{
    public int ShippingAddressId { get; set; }

    public string CustomerId { get; set; } = null!;

    public string FullName { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;

    public AppUser Customer { get; set; } = null!;
}