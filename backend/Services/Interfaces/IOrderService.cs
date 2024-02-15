using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderFromCart(Guid cartId, string userId);
    }
}