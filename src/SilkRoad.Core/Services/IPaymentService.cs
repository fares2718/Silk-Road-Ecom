namespace SilkRoad.Core;

public interface IPaymentService
{
    Task<CustomerBasket> CreateIntentAsync(string basketID,int? delivertMethodId);
}
