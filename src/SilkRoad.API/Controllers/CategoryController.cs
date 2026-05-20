using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SilkRoad.Core;
using SilkRoad.Core.Entities;

namespace SilkRoad.API.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(IUnitOfWork uow, IMapper mapper) : base(uow, mapper){}

        [HttpPost("add-category")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDTO dto)
        {
            if (dto is null)
                return BadRequest("Invalid category data.");
            Category category = _mapper.Map<Category>(dto);
            await _uow.CategoryRepository.AddAsync(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryID }, category);
        }

        [HttpDelete("delete-category/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid category ID.");
            try
            {
                await _uow.CategoryRepository.DeleteAsync(id);
                return Ok("Category deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound($"Category not found: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("all-categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _uow.CategoryRepository.GetAllAsync(c => new CategoryDTO
            (
                c.CategoryID,
                c.CategoryName,
                c.CategoryDescription
            ));
            if (categories is null || !categories.Any())
                return NotFound("No categories found.");

            return Ok(categories);
        }

        [HttpGet("category/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            if(id <= 0)
                return BadRequest("Invalid category ID.");
            var category = await _uow.CategoryRepository.GetByIdAsync<CategoryDTO>(id, c => new CategoryDTO
            (
                c.CategoryID,
                c.CategoryName,
                c.CategoryDescription
            ));
            if (category is null)
                return NotFound("Category not found.");

            return Ok(category);
        }
    
        [HttpPut("update-category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(CategoryDTO dto)
        {
            if (dto is null)
                return BadRequest("Invalid category data.");
            if (dto.CategoryID <= 0)
                return BadRequest("Invalid category ID.");
            Category category = _mapper.Map<Category>(dto);
            try
            {
                await _uow.CategoryRepository.UpdateAsync(category);
                return Ok("Category updated successfully.");
            }
            catch (Exception ex)
            {
                return NotFound($"Category not found: {ex.InnerException?.Message}");
            }
        }
    }
}

