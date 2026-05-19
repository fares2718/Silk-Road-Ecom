using SilkRoad.Core;
using SilkRoad.Core.Entities;

namespace SilkRoad.Infrastructure;

internal class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
}
