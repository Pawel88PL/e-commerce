using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<IEnumerable<ProductDto>>> GetAllProductsAsync();
        Task<ServiceResult<ProductDto>> GetProductAsync(int id);
    }
}
