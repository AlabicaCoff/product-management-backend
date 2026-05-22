using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Api.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
    }
}
