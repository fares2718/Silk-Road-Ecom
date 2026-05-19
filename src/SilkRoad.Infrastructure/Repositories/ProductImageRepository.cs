using SilkRoad.Core;
using SilkRoad.Core.Entities;

namespace SilkRoad.Infrastructure;

internal class ProductImageRepository : BaseRepository<ProductImage>, IProductImageRepository
{
    public ProductImageRepository(AppDbContext context) : base(context)
    {
    }
}
