namespace SilkRoad.Core;

public interface IOrderService
{
    Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync(string? searchTerm = null);
    Task<Order?> GetOrderByIdAsync(Guid orderId, string userId);
    Task<IReadOnlyList<Order>> GetUserOrdersAsync(string userId);
    Task PlaceOrderAsync(PlaceOrderDTO placeOrderDTO, string userId);
}
