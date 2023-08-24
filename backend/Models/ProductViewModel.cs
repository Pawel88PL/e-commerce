namespace MiodOdStaniula.Models
{
    public class ProductViewModel : Product
    {
        public new List<IFormFile> ProductImages { get; set; } = new List<IFormFile>();
        public List<ProductImageInfo> ProductImageInfos { get; set; } = new List<ProductImageInfo>();
    }

    public class ProductImageInfo
    {
        public int ImageId { get; set; }
        public string? ImagePath { get; set; }
    }
}
