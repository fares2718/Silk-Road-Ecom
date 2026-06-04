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
    public async Task<IReadOnlyList<DeliveryMethodDTO>> GetDeliveryMethodsAsync(string? searchTerm = null)
    {
        var query = _context.DeliveryMethods.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(dm => dm.MethodName.Contains(searchTerm) || dm.Provider.ProviderName.Contains(searchTerm));
        }
        return await query
        .Select(x => new DeliveryMethodDTO
        {
            DeliveryMethodId = x.DeliveryMethodId,
            DeliveryTime = x.DeliveryTime,
            Price = x.Price,
            Description = x.Description,
            MethodName = x.MethodName,
            ProviderName = x.Provider.ProviderName,
            Available = x.Available
        })
        .ToListAsync();
    }

    public async Task<OrderDTO?> GetOrderByIdAsync(Guid orderId, string userId)
    {
        Order? order = await _context.Orders
            .AsNoTracking()
            .Select(x => new Order
            {
                OrderId = x.OrderId,
                ShippingAddressSnapshot = x.ShippingAddressSnapshot,
                DeliverySnapshot = x.DeliverySnapshot,
                SubTotal = x.SubTotal,
                Total = x.Total,
                OrderStatus = x.OrderStatus,
                OrderDate = x.OrderDate
            })
            .Include(x => x.OrderItems.Select(x => new OrderItem
            {
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                LineTotal = x.LineTotal
            }))
            .FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerId == userId);
        OrderDTO dto = _mapper.Map<OrderDTO>(order);
        return dto;
    }

    public async Task<IReadOnlyList<OrderDTO>> GetUserOrdersAsync(string userId)
    {
        IReadOnlyList<Order> orders = await _context.Orders
     .AsNoTracking()
     .Where(o => o.CustomerId == userId)
     .OrderBy(x => x.OrderDate)
     .Select(x => new Order
     {
         OrderId = x.OrderId,
         ShippingAddressSnapshot = x.ShippingAddressSnapshot,
         DeliverySnapshot = x.DeliverySnapshot,
         SubTotal = x.SubTotal,
         Total = x.Total,
         OrderStatus = x.OrderStatus,
         OrderDate = x.OrderDate,

         OrderItems = x.OrderItems.Select(oi => new OrderItem
         {
             ProductName = oi.ProductName,
             Quantity = oi.Quantity,
             UnitPrice = oi.UnitPrice,
             LineTotal = oi.LineTotal
         }).ToList()
     })
     .ToListAsync();

        IReadOnlyList<OrderDTO> dtos = _mapper.Map<IReadOnlyList<OrderDTO>>(orders);
        return dtos;
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
        newOrder.Total = newOrder.SubTotal + newOrder.DeliverySnapshot.DeliveryPrice;
        newOrder.OrderItems = basket.BasketItems.Select(item => new OrderItem
        {
            ProductId = item.ItemID,
            ProductName = item.ItemName,
            UnitPrice = item.Price,
            Quantity = item.Quantity,
            LineTotal = item.Price * item.Quantity
        }).ToList();

        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();
        await _uow.CustomerBasketRepository.DeleteBasketAsync(basket.BasketID);
    }
}


