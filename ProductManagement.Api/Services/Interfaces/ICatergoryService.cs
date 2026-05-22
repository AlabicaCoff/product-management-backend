using ProductManagement.Api.DTOs;

namespace ProductManagement.Api.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task<CategoryDto> CreateAsync(CategoryRequestDto categoryRequestDto);
        Task UpdateAsync(Guid id, CategoryRequestDto categoryRequestDto);
        Task DeleteAsync(Guid id);
    }
}
