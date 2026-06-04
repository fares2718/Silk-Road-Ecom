namespace SilkRoad.Core;

public interface IPaymentService
{
    Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketID,int? delivertMethodId);
}
