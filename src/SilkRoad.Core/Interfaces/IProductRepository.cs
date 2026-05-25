using SilkRoad.Core.Entities;

namespace SilkRoad.Core;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<bool> AddAsync(AddProductDTO product);

    Task<IReadOnlyList<ProductDTO>> GetAllAsync(string? sortBy = null, bool isDescending = false);

    Task<bool> UpdateAsync(UpdateProductDTO product);
}
