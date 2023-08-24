using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IDeleteService
    {
        Task<bool> DeleteProductAsync(int ProductId);
    }
}
