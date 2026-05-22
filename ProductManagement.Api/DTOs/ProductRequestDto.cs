namespace ProductManagement.Api.DTOs
{
    public class ProductRequestDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public List<Guid> CategoryIds { get; set; } = new();
    }
}
