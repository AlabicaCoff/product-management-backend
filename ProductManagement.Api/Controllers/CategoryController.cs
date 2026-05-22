using ProductManagement.Api.DTOs;
using ProductManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProductManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        // POST: {apiBaseUrl}/api/category
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CategoryRequestDto categoryRequestDto)
        {
            var category = await _categoryService.CreateAsync(categoryRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        // GET: {apiBaseUrl}/api/category
        [HttpGet]
        [Authorize(Roles = "Admin,Non-Admin")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        // GET: {apiBaseUrl}/api/category/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Non-Admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // PUT: {apiBaseUrl}/api/category/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryRequestDto categoryRequestDto)
        {
            var isUpdated = await _categoryService.UpdateAsync(id, categoryRequestDto);
            if (!isUpdated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: {apiBaseUrl}/api/category/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isDeleted = await _categoryService.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

