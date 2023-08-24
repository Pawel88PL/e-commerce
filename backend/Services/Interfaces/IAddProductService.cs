using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IAddProductService
    {
        Task<Product> AddNewProductAsync(Product product);
    }
}
