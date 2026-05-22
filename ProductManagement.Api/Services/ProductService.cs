using AutoMapper;
using ProductManagement.Api.Repositories.Interfaces;
using ProductManagement.Api.Services.Interfaces;
using ProductManagement.Api.Models;
using ProductManagement.Api.DTOs;


namespace ProductManagement.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        this._productRepository = productRepository;
        this._mapper = mapper;
    }

    public async Task<ProductDto> CreateAsync(ProductRequestDto productRequestDto)
    {
        var product = _mapper.Map<Product>(productRequestDto);

        product.ProductCategories = productRequestDto.CategoryIds.Select(categoryId => new ProductCategory
        {
            // Notice we only need the child ID. EF Core handles the parent ID.
            CategoryId = categoryId 
        }).ToList();

        var response = await _productRepository.CreateAsync(product);
        var createdProduct = await _productRepository.GetByIdAsync(response.Id);
        var productResponse = _mapper.Map<ProductDto>(createdProduct);
        return productResponse;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }
        await _productRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<ProductDto>> GetAllPaginationAsync(ProductPaginationRequestDto paginationRequestDto)
    {
        var products = await _productRepository.GetAllPaginationAsync(paginationRequestDto);
        var productList = products.ToList();
        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(productList);
        return productDtos;
    }

    public async Task<bool> UpdateAsync(Guid id, ProductRequestDto productRequestDto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        _mapper.Map(productRequestDto, product);

        var updatedCategoryIds = productRequestDto.CategoryIds;
        var existingCategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList();

        var categoriesToRemove = product.ProductCategories
            .Where(pc => !updatedCategoryIds.Contains(pc.CategoryId))
            .ToList();

        foreach (var category in categoriesToRemove)
        {
            product.ProductCategories.Remove(category);
        }

        var categoriesToAdd = updatedCategoryIds
            .Where(id => !existingCategoryIds.Contains(id))
            .Select(id => new ProductCategory { CategoryId = id }) // EF handles the ProductId automatically
            .ToList();

        foreach (var category in categoriesToAdd)
        {
            product.ProductCategories.Add(category);
        }

        await _productRepository.UpdateAsync(product);
        return true;
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return null;
        }
        var productDto = _mapper.Map<ProductDto>(product);
        return productDto;
    }
}