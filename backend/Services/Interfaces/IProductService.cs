using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<IEnumerable<ProductDisplayDto>>> GetAllProductsAsync();
        Task<ServiceResult<ProductDisplayDto>> GetProductAsync(int id);
    }
}
