using System.Text.Json;
using SilkRoad.Core;
using StackExchange.Redis;

namespace SilkRoad.Infrastructure;

internal class CustomerBasketRepository : ICustomerBasketRepository
{
    private readonly IDatabase _database;
    public CustomerBasketRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }
    public async Task<CustomerBasket?> AddUpdateBasketAsync(CustomerBasket Basket)
    {
        var result = await _database.StringSetAsync(Basket.BasketID,JsonSerializer.Serialize(Basket));
        if(!result)
            return null;
        RedisValue basket = await _database.StringGetAsync(Basket.BasketID);
        return JsonSerializer.Deserialize<CustomerBasket>(basket!);
    }

    public async Task<bool> DeleteBasketAsync(string Id)
    {
        bool result = await _database.KeyDeleteAsync(Id);
        return result;
    }

    public async Task<CustomerBasket?> GetBasketByIdAsync(string Id)
    {
        RedisValue result = await _database.StringGetAsync(Id);
        if(string.IsNullOrEmpty(result))
            return null;
        return JsonSerializer.Deserialize<CustomerBasket>(result!);
    }

}
