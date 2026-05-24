using ProductManagement.Api.Models;
using ProductManagement.Api.DTOs;

namespace ProductManagement.Api.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<(IEnumerable<Product> Products, int TotalCount)> GetAllPaginationAsync(ProductPaginationRequestDto paginationRequestDto);
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product> CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
    }
}
