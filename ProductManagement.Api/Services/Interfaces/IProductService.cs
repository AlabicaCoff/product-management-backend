using ProductManagement.Api.DTOs;

namespace ProductManagement.Api.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllPaginationAsync(ProductPaginationRequestDto paginationRequestDto);
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<ProductDto> CreateAsync(ProductRequestDto productRequestDto);
        Task<bool> UpdateAsync(Guid id, ProductRequestDto productRequestDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
