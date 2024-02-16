using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO?> CreateOrderFromCart(Guid cartId, string userId);
    }
}