using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Guid?> CreateOrderFromCart(Guid cartId, string userId);
        Task<OrderDTO?> GetOrderDetails(Guid orderId);
        Task<List<OrderHistoryDTO>> GetOrdersHistory(string userId);
    }
}