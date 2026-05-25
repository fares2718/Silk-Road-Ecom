using SilkRoad.Core.Entities;

namespace SilkRoad.Core;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<bool> AddAsync(AddProductDTO product);

    Task<bool> UpdateAsync(UpdateProductDTO product);
}
