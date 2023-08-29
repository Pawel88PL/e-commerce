
namespace MiodOdStaniula
{
    public class ProductDisplayDto
    {
        public int ProductId { get; set; }
        public int Priority { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Weight { get; set; }
        public int AmountAvailable { get; set; }
        public int? Popularity { get; set; }
        public DateTime DateAdded { get; set; }
        public List<ProductImageDto> ProductImages { get; set; } = new List<ProductImageDto>();

        public int? CategoryId { get; set; }
        public string? Category { get; set; }
    }

    public class ProductImageDto
    {
        public int ImageId { get; set; }
        public string? ImagePath { get; set; }
    }

}