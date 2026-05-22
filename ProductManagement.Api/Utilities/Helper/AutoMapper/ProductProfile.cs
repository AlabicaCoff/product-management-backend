using AutoMapper;
using ProductManagement.Api.Models;
using ProductManagement.Api.DTOs;

namespace ProductManagement.Api.Utilities.Helper.AutoMapper;
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // ProductRequestDto -> Product
        CreateMap<ProductRequestDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());


        // Product -> ProductDto
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(
                src => src.ProductCategories != null
                    ? src.ProductCategories
                        .Where(pc => pc != null && pc.Category != null)
                        .Select(pc => pc.Category!.Name)
                        .ToList()
                    : new List<string>()
            ));
    }
}