using ProductManagement.Api.Data;
using ProductManagement.Api.Models;
using ProductManagement.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Api.DTOs;
using ProductManagement.Api.Enums;

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

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetAllPaginationAsync(ProductPaginationRequestDto paginationRequestDto)
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

            bool isAscending = paginationRequestDto.OrderDirection == ProductOrderDirectionEnum.Ascending;
            query = paginationRequestDto.OrderBy switch
            {   
                ProductOrderByEnum.Name => isAscending 
                    ? query.OrderBy(p => p.Name) 
                    : query.OrderByDescending(p => p.Name),
                    
                ProductOrderByEnum.Price => isAscending 
                    ? query.OrderBy(p => p.Price) 
                    : query.OrderByDescending(p => p.Price),
                    
                ProductOrderByEnum.Stock => isAscending 
                    ? query.OrderBy(p => p.Stock) 
                    : query.OrderByDescending(p => p.Stock),

                ProductOrderByEnum.CreatedAt => isAscending 
                    ? query.OrderBy(p => p.CreatedDate) 
                    : query.OrderByDescending(p => p.CreatedDate),
                    
                ProductOrderByEnum.UpdatedAt => isAscending 
                    ? query.OrderBy(p => p.UpdatedDate) 
                    : query.OrderByDescending(p => p.UpdatedDate),
                
                _ => isAscending 
                    ? query.OrderBy(p => p.CreatedDate) 
                    : query.OrderByDescending(p => p.CreatedDate) 
            };

            // Pagination Tie-Breaker
            query = ((IOrderedQueryable<Product>)query).ThenBy(p => p.Id);
            
            var totalItems = await query.CountAsync();

            var products = await query
                .Skip((paginationRequestDto.PageNumber - 1) * paginationRequestDto.PageSize)
                .Take(paginationRequestDto.PageSize)
                .ToListAsync();

            return (products, totalItems);
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
