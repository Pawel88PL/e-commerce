using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IAddNewProductService
    {
        Task<ServiceResult<Product>> AddNewProductAsync(Product product, List<string> imagePaths);
    }
}
