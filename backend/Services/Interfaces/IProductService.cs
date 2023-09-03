using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<Product>> AddAsync(Product product, List<string> imagePaths);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<IEnumerable<ProductDisplayDto>>> GetAllProductsAsync();
        Task<ServiceResult<ProductDisplayDto>> GetProductAsync(int id);
        Task<ServiceResult<Product>> UpdateAsync(int id, Product updatedProduct);
    }
}
