using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IEditProductService
    {
        Task<(bool IsSuccess, Exception? Error)> UpdateProductAsync(Product product);
    }
}
