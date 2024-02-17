using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int?> CreateOrderFromCart(Guid cartId, string userId);
        Task<OrderDTO?> GetOrderDetails(int orderId);
    }
}