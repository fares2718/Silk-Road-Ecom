using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SilkRoad.Core;
using Stripe;

namespace SilkRoad.Infrastructure;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _config;
    private readonly AppDbContext _context;

    public PaymentService(IUnitOfWork uow, IConfiguration config, AppDbContext context)
    {
        _uow = uow;
        _config = config;
        _context = context;
    }

    public async Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketID, int? delivertMethodId)
    {
        CustomerBasket? basket = await _uow.CustomerBasketRepository.GetBasketByIdAsync(basketID);

        StripeConfiguration.ApiKey = _config["Stiper:sectretKey"];

        decimal shippingPrice = 0;

        if (delivertMethodId.HasValue)
        {
            var delivery = await _context.DeliveryMethods.AsNoTracking()
                .FirstOrDefaultAsync(m => m.DeliveryMethodId == delivertMethodId.Value);
            shippingPrice = delivery!.Price;
        }

        // 1. Extract all unique IDs from the basket to minimize query size
        var productIds = basket!.BasketItems
            .Select(item => item.ItemID)
            .Distinct()
            .ToList();

        // 2. Fetch all matching product prices in a single, fast database query
        var productPrices = await _context.Products
            .AsNoTracking()
            .Where(p => productIds.Contains(p.ProductID))
            .Select(p => new { p.ProductID, p.NewPrice })
            .ToDictionaryAsync(p => p.ProductID, p => p.NewPrice);

        // 3. Update the basket items using O(1) in-memory dictionary lookups
        foreach (var item in basket.BasketItems)
        {
            if (productPrices.TryGetValue(item.ItemID, out var currentPrice))
            {
                item.Price = currentPrice;
            }
        }

        PaymentIntentService paymentIntentService = new PaymentIntentService();
        PaymentIntent _intent;
        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var option = new PaymentIntentCreateOptions
            {
                Amount = (long)basket.BasketItems.Sum(bi => bi.Quantity * (bi.Price * 100)) + (long)(shippingPrice * 100),

                Currency = "USD",
                PaymentMethodTypes = new List<string> { "card" }
            };
            _intent = await paymentIntentService.CreateAsync(option);
            basket.PaymentIntentId = _intent.Id;
            basket.ClientSecret = _intent.ClientSecret;
        }
        else
        {
            var option = new PaymentIntentUpdateOptions
            {
                Amount = (long)basket.BasketItems.Sum(bi => bi.Quantity * (bi.Price * 100)) + (long)(shippingPrice * 100),

            };
            await paymentIntentService.UpdateAsync(basket.PaymentIntentId, option);
        }
        await _uow.CustomerBasketRepository.AddUpdateBasketAsync(basket);
        return basket;
    }
}
