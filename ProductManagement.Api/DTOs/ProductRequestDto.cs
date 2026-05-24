using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Api.DTOs
{
    public class ProductRequestDto
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Product name cannot be empty.")]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }
        public List<Guid> CategoryIds { get; set; } = new();
    }
}
