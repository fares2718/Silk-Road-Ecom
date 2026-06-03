namespace SilkRoad.Core;

public interface IOrderService
{
    Task<IReadOnlyList<DeliveryMethodDTO>> GetDeliveryMethodsAsync(string? searchTerm = null);
    Task<OrderDTO?> GetOrderByIdAsync(Guid orderId, string userId);
    Task<IReadOnlyList<OrderDTO>> GetUserOrdersAsync(string userId);
    Task PlaceOrderAsync(PlaceOrderDTO placeOrderDTO, string userId);
}
