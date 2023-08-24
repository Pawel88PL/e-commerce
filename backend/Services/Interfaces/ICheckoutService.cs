using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<Order?> CreateOrderAsync(Guid sessionId);
    }
}