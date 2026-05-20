using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SilkRoad.Core;
using SilkRoad.Core.Entities;
using SilkRoad.API;

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
                return BadRequest(new APIResponse(400));
            try
            {
                await _uow.CategoryRepository.DeleteAsync(id);
                return Ok(new APIResponse(200));
            }
            catch (Exception)
            {
                return NotFound(new APIResponse(404));
                throw;
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
                return NotFound(new APIResponse(404));

            return Ok(categories);
        }
        
        [HttpGet("category/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            if(id <= 0)
                return BadRequest(new APIResponse(400));
            var category = await _uow.CategoryRepository.GetByIdAsync<CategoryDTO>(id, c => new CategoryDTO
            (
                c.CategoryID,
                c.CategoryName,
                c.CategoryDescription
            ));
            if (category is null)
                return NotFound(new APIResponse(404));

            return Ok(category);
        }
    
        [HttpPut("update-category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(CategoryDTO dto)
        {
            if (dto is null)
                return BadRequest(new APIResponse(400));
            if (dto.CategoryID <= 0)
                return BadRequest(new APIResponse(400));
            Category category = _mapper.Map<Category>(dto);
            try
            {
                await _uow.CategoryRepository.UpdateAsync(category);
                return Ok(new APIResponse(200));
            }
            catch (Exception)
            {
                return NotFound(new APIResponse(404));
                throw;
            }
        }
    }
}

