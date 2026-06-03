using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SilkRoad.Core;

namespace SilkRoad.Infrastructure;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _uow;
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork uow, AppDbContext context, IMapper mapper, UserManager<AppUser> userManager)
    {
        _uow = uow;
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }
    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync(string? searchTerm = null)
    {
        var query = _context.DeliveryMethods.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(dm => dm.MethodName.Contains(searchTerm) || dm.Provider.ProviderName.Contains(searchTerm));
        }
        return await query.ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId, string userId)
    {
        return await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerId == userId);
    }

    public async Task<IReadOnlyList<Order>> GetUserOrdersAsync(string userId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId == userId)
            .ToListAsync();
    }

    public async Task PlaceOrderAsync(PlaceOrderDTO placeOrderDTO, string userId)
    {
        var basket = await _uow.CustomerBasketRepository
                    .GetBasketByIdAsync(placeOrderDTO.BasketID);
        if (basket == null || basket.BasketItems.Count == 0)
        {
            throw new Exception("Basket is empty or does not exist.");
        }
        var insertOrderQuery = _userManager.Users
            .Where(u => u.Id == userId)
            .SelectMany(
                u => _context.DeliveryMethods.Where(dm => dm.DeliveryMethodId == placeOrderDTO.DeliveryMethodID),
                (u, dm) => new Order
                {
                    CustomerId = u.Id,
                    ShippingAddressSnapshot = new ShippingAddressSnapshot
                    {
                        ShippingFullName = $"{u.FirstName} {u.MiddleName ?? ""} {u.LastName}",
                        ShippingStreet = u.AppUserInfo!.Street ?? "",
                        ShippingCity = u.AppUserInfo!.City.CityName,
                        ShippingPostalCode = u.AppUserInfo!.ZipCode,
                        ShippingCountry = u.AppUserInfo!.City.State.Country.CountryName,
                    },
                    DeliverySnapshot = new DeliverySnapshot
                    {
                        DeliveryProviderName = dm.Provider.ProviderName ?? "",
                        DeliveryMethodName = dm.MethodName ?? "",
                        DeliveryPrice = dm.Price
                    },
                }
            );

        var newOrder = await insertOrderQuery.FirstOrDefaultAsync();
        if (newOrder == null)
        {
            throw new Exception("Failed to create order.");
        }
        newOrder.SubTotal = basket.BasketItems.Sum(item => item.Price * item.Quantity);
        newOrder.OrderItems = basket.BasketItems.Select(item => new OrderItem
        {
            ProductId = item.ItemID,
            ProductName = item.ItemName,
            UnitPrice = item.Price,
            Quantity = item.Quantity
        }).ToList();

        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();
        await _uow.CustomerBasketRepository.DeleteBasketAsync(basket.BasketID);
    }
}


