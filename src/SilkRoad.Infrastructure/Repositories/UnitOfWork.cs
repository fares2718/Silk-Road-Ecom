using SilkRoad.Core;

namespace SilkRoad.Infrastructure;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public ICategoryRepository CategoryRepository { get; }

    public IProductRepository ProductRepository { get; }

    public IProductImageRepository ProductImageRepository { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        CategoryRepository = new CategoryRepository(_context);
        ProductRepository = new ProductRepository(_context);
        ProductImageRepository = new ProductImageRepository(_context);
    }

}
