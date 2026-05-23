using ProductManagement.Api.Data;
using ProductManagement.Api.Models;
using ProductManagement.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Api.DTOs;

namespace ProductManagement.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllPaginationAsync(ProductPaginationRequestDto paginationRequestDto)
        {
            var query = _dbContext.Products.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).AsQueryable();

            if (!string.IsNullOrEmpty(paginationRequestDto.Search))
            {
                query = query.Where(p => p.Name.Contains(paginationRequestDto.Search));
            }

            if (paginationRequestDto.Categories.Any())
            {
                query = query.Where(p => p.ProductCategories
                    .Any(pc => paginationRequestDto.Categories.Contains(pc.CategoryId)));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)paginationRequestDto.PageSize);

            var products = await query
                .Skip((paginationRequestDto.PageNumber - 1) * paginationRequestDto.PageSize)
                .Take(paginationRequestDto.PageSize)
                .ToListAsync();

            return products;
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            var product = await _dbContext.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
