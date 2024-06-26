using backend.Models;

namespace backend.Interfaces
{
    public interface IOrderService
    {
        Task<Guid?> CreateOrderFromCart(Guid cartId, string userId, bool isPickupInStore);
        Task<List<AdminOrderDTO>> GetAllOrders();
        Task<OrderDTO?> GetOrderDetails(Guid orderId);
        Task<List<OrderHistoryDTO>> GetOrdersHistory(string userId);
        Task<bool> UpdateOrderStatus(Guid orderId, string newStatus);
    }
}