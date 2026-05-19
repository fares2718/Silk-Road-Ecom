using SilkRoad.Core;
using SilkRoad.Core.Entities;

namespace SilkRoad.Infrastructure;

internal class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}
