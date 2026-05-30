using AutoMapper;
using SilkRoad.Core;
using SilkRoad.Core.Services;
using StackExchange.Redis;

namespace SilkRoad.Infrastructure;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IImageManagementService _imageManagementService;
    private readonly IMapper _mapper;
    private readonly IConnectionMultiplexer _redis;
    public ICategoryRepository CategoryRepository { get; }

    public IProductRepository ProductRepository { get; }

    public IProductImageRepository ProductImageRepository { get; }

    public ICustomerBasketRepository CustomerBasketRepository { get; }

    public UnitOfWork(AppDbContext context, IImageManagementService imageManagementService,
         IMapper mapper, IConnectionMultiplexer redis)
    {
        _context = context;
        _imageManagementService = imageManagementService;
        _mapper = mapper;
        _redis = redis;
        CategoryRepository = new CategoryRepository(_context);
        ProductRepository = new ProductRepository(_context, _mapper, _imageManagementService);
        ProductImageRepository = new ProductImageRepository(_context);
        CustomerBasketRepository = new CustomerBasketRepository(_redis);
    }

}
