using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SilkRoad.Core;
using SilkRoad.Core.Entities;
using SilkRoad.Core.Services;

namespace SilkRoad.Infrastructure;

internal class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    private readonly IImageManagementService _imageManagementService;
    public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
    {
        _context = context;
        _mapper = mapper;
        _imageManagementService = imageManagementService;
    }

    public async Task<bool> AddAsync(AddProductDTO product)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");
        Product newProduct = _mapper.Map<Product>(product);

        List<string> imageUrls = await _imageManagementService.UploadImagesAsync(product.ProductImages, newProduct.ProductName);

        List<ProductImage> productImages = imageUrls
        .Select(url => new ProductImage { ImageURL = url, ProductID = newProduct.ProductID })
        .ToList();

        newProduct.ProductImages = productImages;

        _context.Products.Add(newProduct);

        int rowsAffected = await _context.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public override async Task DeleteAsync(int id)
    {
        Product? product = await _context.Products
        .Include(p => p.ProductImages)
        .FirstOrDefaultAsync(p => p.ProductID == id);

        if (product is null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        List<string> imageUrls = product.ProductImages.Select(pi => pi.ImageURL).ToList();
        _imageManagementService.DeleteImagesAsync($"Images/{product.ProductName}");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<ProductDTO>>
     GetAllAsync(ProductParams productParams)
    {
        IQueryable<Product> query = _context.Products
        .Include(p => p.ProductImages)
        .Include(p => p.Category)
        .AsNoTracking();

        if (productParams.CategoryID.HasValue && productParams.CategoryID is not 0)
            query = query.Where(p => p.CategoryID == productParams.CategoryID);
        if(!string.IsNullOrEmpty(productParams.Search))
            query = query.Where(p => p.ProductName.ToLower()
            .Contains(productParams.Search.ToLower()) ||
            p.Description!.ToLower()
            .Contains(productParams.Search.ToLower())
            );

        if (!string.IsNullOrEmpty(productParams.SortBy))
        {
            query = productParams.SortBy.ToLower() switch
            {
                "name" => productParams.IsDescending ? query
                .OrderByDescending(p => p.ProductName) : 
                query.OrderBy(p => p.ProductName),
                "price" => productParams.IsDescending ? query
                .OrderByDescending(p => p.NewPrice) : 
                query.OrderBy(p => p.NewPrice),
                _ => query
            };
        }

        productParams.TotalCount = await query.CountAsync();
        return await query
        .Skip((productParams.PageNumber - 1) * productParams.PageSize)
        .Take( productParams.PageSize)
        .Select(p => new ProductDTO(
            p.ProductID,
            p.ProductName,
            p.Description,
            p.Category.CategoryName,
            p.NewPrice,
            p.OldPrice,
            p.ProductImages.Select(pi => pi.ImageURL).ToList()
        ))
        .ToListAsync();
    }

    public async Task<bool> UpdateAsync(UpdateProductDTO product)
    {
        Product? existingProduct = await _context.Products
        .Include(p => p.ProductImages)
        .FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

        if (existingProduct is null)
            return false;
        if (product.NewPrice is not 0)
            existingProduct.OldPrice = existingProduct.NewPrice;
        if (product.ProductImages is not null && product.ProductImages.Count > 0)
        {
            List<string> oldImageUrls = existingProduct.ProductImages.Select(pi => pi.ImageURL).ToList();

            _imageManagementService.DeleteImagesAsync($"Images/{existingProduct.ProductName}");

            _context.ProductImages.RemoveRange(existingProduct.ProductImages);

            List<string> newImageUrls = await _imageManagementService
            .UploadImagesAsync(product.ProductImages, existingProduct.ProductName);

            List<ProductImage> newProductImages = newImageUrls
            .Select(url => new ProductImage { ImageURL = url, ProductID = existingProduct.ProductID })
            .ToList();

            _context.ProductImages.AddRange(newProductImages);
        }
        _mapper.Map(product, existingProduct);
        int rowsAffected = await _context.SaveChangesAsync();
        return rowsAffected > 0;
    }
}
