using AutoMapper;
using ProductManagement.Api.Repositories.Interfaces;
using ProductManagement.Api.Services.Interfaces;
using ProductManagement.Api.Models;
using ProductManagement.Api.DTOs;


namespace ProductManagement.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        this._categoryRepository = categoryRepository;
        this._mapper = mapper;
    }

    public async Task<CategoryDto> CreateAsync(CategoryRequestDto categoryRequestDto)
    {
        var category = _mapper.Map<Category>(categoryRequestDto);
        var response = await _categoryRepository.CreateAsync(category);

        var categoryResponse = _mapper.Map<CategoryDto>(response);
        return categoryResponse;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return false;
        }
        await _categoryRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var categoryList = categories?.ToList() ?? new List<Category>();
        var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categoryList);
        return categoryDtos;
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return null;
        }
        var categoryDto = _mapper.Map<CategoryDto>(category);
        return categoryDto;
    }

    public async Task<bool> UpdateAsync(Guid id, CategoryRequestDto categoryRequestDto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return false;
        }
        var updatedCategory = _mapper.Map(categoryRequestDto, category);
        category.Id = id;
        await _categoryRepository.UpdateAsync(updatedCategory);
        return true;
    }
}