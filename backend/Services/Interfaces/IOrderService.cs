using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Guid?> CreateOrderFromCart(Guid cartId, string userId, bool isPickupInStore);
        Task<OrderDTO?> GetOrderDetails(Guid orderId);
        Task<List<OrderHistoryDTO>> GetOrdersHistory(string userId);
    }
}