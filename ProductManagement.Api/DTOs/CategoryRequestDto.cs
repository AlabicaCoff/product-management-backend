using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Api.DTOs
{
    public class CategoryRequestDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Category name cannot be empty.")]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
