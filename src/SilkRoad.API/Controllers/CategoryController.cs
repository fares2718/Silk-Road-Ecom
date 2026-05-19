using Microsoft.AspNetCore.Mvc;
using SilkRoad.Core;
using SilkRoad.Core.Entities;

namespace MyApp.Namespace
{
    public class CategoryController : BaseController
    {
        public CategoryController(IUnitOfWork uow) : base(uow){}

        [HttpPost("add-category")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDTO dto)
        {
            if (dto is null)
                return BadRequest("Invalid category data.");
            Category category = new Category
            {
                CategoryName = dto.CategoryName,
                CategoryDescription = dto.CategoryDescription
            };
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
            var categories = await _uow.CategoryRepository.GetAllAsync();
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
            var category = await _uow.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound("Category not found.");

            return Ok(category);
        }
    
        [HttpPut("update-category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO dto)
        {
            if (dto is null)
                return BadRequest("Invalid category data.");
            if (dto.CategoryID <= 0)
                return BadRequest("Invalid category ID.");
            Category category = new Category
            {
                CategoryID = dto.CategoryID,
                CategoryName = dto.CategoryName,
                CategoryDescription = dto.CategoryDescription
            };
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

