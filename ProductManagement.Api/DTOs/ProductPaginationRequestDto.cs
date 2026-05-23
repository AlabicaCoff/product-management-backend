using ProductManagement.Api.Enums;

namespace ProductManagement.Api.DTOs
{
    public class ProductPaginationRequestDto
    {
        public ProductOrderByEnum OrderBy { get; set; } = ProductOrderByEnum.CreatedAt;
        public ProductOrderDirectionEnum OrderDirection { get; set; } = ProductOrderDirectionEnum.Descending;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public List<Guid> Categories { get; set; } = new List<Guid>();
    }
}
