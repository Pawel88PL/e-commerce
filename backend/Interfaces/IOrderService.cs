using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Interfaces
{
    public interface IOrderService
    {
        Task<string?> CreateOrderFromCart(CreateOrder createOrder);
        Task<List<AdminOrderDTO>> GetAllOrders();
        Task<OrderDTO?> GetOrderDetails(Guid orderId);
        Task<List<OrderHistoryDTO>> GetOrdersHistory(string userId);
        Task<bool> UpdateOrderStatus(Guid orderId, string newStatus);
    }
}