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
        if(product is null)
            throw new ArgumentNullException(nameof(product),"Product cannot be null.");
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
        Product? product = await _context.Products.FindAsync(id);
        if (product is null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        List<string> imageUrls = product.ProductImages.Select(pi => pi.ImageURL).ToList();
        foreach (string url in imageUrls)
        {
            _imageManagementService.DeleteImagesAsync(url);
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(UpdateProductDTO product)
    {
        Product? existingProduct = await _context.Products
        .Include(p => p.ProductImages)
        .FirstOrDefaultAsync(p => p.ProductID == product.ProductID);
        
        if(existingProduct is null)
            return false;
        if(product.NewPrice is not 0)
            existingProduct.OldPrice = existingProduct.NewPrice;
        if(product.ProductImages is not null && product.ProductImages.Count > 0)
        {
            List<string> oldImageUrls = existingProduct.ProductImages.Select(pi => pi.ImageURL).ToList();
            foreach (string url in oldImageUrls)
            {
                _imageManagementService.DeleteImagesAsync(url);
            }
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
