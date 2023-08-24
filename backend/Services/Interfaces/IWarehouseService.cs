using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IWarehouseService
    {
        Task<Product?> GetProductAsync(int ProductId);

        Task<List<Product>> GetAllProductsAsync();
    }
}
