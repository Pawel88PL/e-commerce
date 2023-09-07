namespace MiodOdStaniula.Models
{
    public class ProductDto
    {
        public int AmountAvailable { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime DateAdded { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public int Priority { get; set; }
        public decimal Price { get; set; }
        public int? Popularity { get; set; }
        public int ProductId { get; set; }
        public int Weight { get; set; }
        public List<string> ImagePaths { get; set; } = new List<string>();
        public List<ProductImageDto> ProductImages { get; set; } = new List<ProductImageDto>();
    }
}
