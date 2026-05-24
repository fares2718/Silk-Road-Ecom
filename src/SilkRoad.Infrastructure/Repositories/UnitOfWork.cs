using AutoMapper;
using SilkRoad.Core;
using SilkRoad.Core.Services;

namespace SilkRoad.Infrastructure;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IImageManagementService _imageManagementService;
    private readonly IMapper _mapper;
    public ICategoryRepository CategoryRepository { get; }

    public IProductRepository ProductRepository { get; }

    public IProductImageRepository ProductImageRepository { get; }

    public UnitOfWork(AppDbContext context, IImageManagementService imageManagementService, IMapper mapper)
    {
        _context = context;
        _imageManagementService = imageManagementService;
        _mapper = mapper;
        CategoryRepository = new CategoryRepository(_context);
        ProductRepository = new ProductRepository(_context, _mapper, _imageManagementService);
        ProductImageRepository = new ProductImageRepository(_context);
    }

}
