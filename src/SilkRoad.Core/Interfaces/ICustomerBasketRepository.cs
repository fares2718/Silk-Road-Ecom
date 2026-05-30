namespace SilkRoad.Core;

public interface ICustomerBasketRepository
{
    Task<CustomerBasket?> AddUpdateBasketAsync(CustomerBasket Basket);
    Task<bool> DeleteBasketAsync(string Id);
    Task<CustomerBasket?> GetBasketByIdAsync(string Id);
}
