using backend.Models;
using backend.Services;

namespace backend.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<ProductDto>> AddAsync(ProductDto productDto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<IEnumerable<ProductDto>>> GetAllProductsAsync();
        Task<ServiceResult<PaginatedList<ProductDto>>> GetAllProductsAsync(int page, int itemsPerPage);
        Task<ServiceResult<ProductDto>> GetProductAsync(int id);
        Task<ServiceResult<ProductDto>> UpdateAsync(int id, ProductDto updatedProduct);
    }
}
