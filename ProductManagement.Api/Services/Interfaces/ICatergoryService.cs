using ProductManagement.Api.DTOs;

namespace ProductManagement.Api.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task<CategoryDto> CreateAsync(CategoryRequestDto categoryRequestDto);
        Task<bool> UpdateAsync(Guid id, CategoryRequestDto categoryRequestDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
