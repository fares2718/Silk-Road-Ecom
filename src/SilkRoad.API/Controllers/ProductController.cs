using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SilkRoad.API.Controllers;
using SilkRoad.Core;
using Microsoft.EntityFrameworkCore;
using SilkRoad.Core.Entities;

namespace SilkRoad.API;

public class ProductController : BaseController
{
    public ProductController(IUnitOfWork uow, IMapper mapper) : base(uow, mapper){}

    [HttpPost("add-product")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> AddProduct([FromQuery]AddProductDTO productDTO)
    {
        if (productDTO is null)
            return BadRequest(new APIResponse(400));
        if(!await _uow.ProductRepository.AddAsync(productDTO))
            return BadRequest(new APIResponse(400));
        else
            return Created("", new APIResponse(201));
    }

    [HttpDelete("delete-product/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (id <= 0)
            return BadRequest(new APIResponse(400));
        try
        {
            await _uow.ProductRepository.DeleteAsync(id);
            return Ok(new APIResponse(200));
        }
        catch (Exception)
        {
            return NotFound(new APIResponse(404));
            throw;
        }
    }

    [HttpGet("all-products")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllProducts()
    {
        IReadOnlyList<ProductDTO> products = await _uow.ProductRepository
        .GetAllAndIncludeAsync(p => new ProductDTO
        (
            p.ProductID,
            p.ProductName,
            p.Description,
            p.Category.CategoryName,
            p.NewPrice,
            p.OldPrice,
            p.ProductImages.Select(pi => pi.ImageURL).ToList()
        ),
        p => p.CategoryID);

        if (products is null || !products.Any())
            return NotFound(new APIResponse(404));
        return Ok(products);
    }

    [HttpGet("product/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(int id)
    {
        if (id <= 0)
            return BadRequest(new APIResponse(400));
        ProductDTO? product = await _uow.ProductRepository.GetByIdAndIncludeAsync(id,p => new ProductDTO
        (
            p.ProductID,
            p.ProductName,
            p.Description,
            p.Category.CategoryName,
            p.NewPrice,
            p.OldPrice,
            p.ProductImages.Select(pi => pi.ImageURL).ToList()
        ));

        if (product is null)
            return NotFound(new APIResponse(404));
        return Ok(product);
    }

    [HttpPut("update-product")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct([FromQuery] UpdateProductDTO productDTO)
    {
        if (productDTO is null || productDTO.ProductID <= 0)
            return BadRequest(new APIResponse(400));

        if(!await _uow.ProductRepository.UpdateAsync(productDTO))
            return NotFound(new APIResponse(404));

        return Ok(new APIResponse(200));
    }
}
