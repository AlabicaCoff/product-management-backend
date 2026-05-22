using ProductManagement.Api.DTOs;
using ProductManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProductManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        // POST: {apiBaseUrl}/api/product
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ProductRequestDto productRequestDto)
        {
            var product = await _productService.CreateAsync(productRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // GET: {apiBaseUrl}/api/product
        [HttpGet]
        [Authorize(Roles = "Admin,Non-Admin")]
        public async Task<IActionResult> GetPagination([FromQuery] ProductPaginationRequestDto paginationRequestDto)
        {
            var products = await _productService.GetAllPaginationAsync(paginationRequestDto);
            return Ok(products);
        }

        // GET: {apiBaseUrl}/api/product/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Non-Admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // PUT: {apiBaseUrl}/api/product/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductRequestDto product)
        {
            var isUpdated = await _productService.UpdateAsync(id, product);
            if (!isUpdated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: {apiBaseUrl}/api/product/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isDeleted = await _productService.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

