using backend.Models;

namespace backend.Interfaces
{
    public interface ICartService
    {
        Task AddItemToCart(Guid cartId, int productId, int quantity);
        Task AssignCartToUser(Guid cartId, Guid userId);
        Task<bool> ClearCartAsync(Guid cartId);
        Task<bool> DeleteItemFromCartAsync(Guid cartId, int productId);
        Task<CartDto?> GetCartAsync(Guid ShopingCartId);
        Task<int> GetCartItemCount(Guid cartId);
        Task<bool> UpdateCartItemQuantityAsync(Guid cartId, int productId, int quantity);
    }
}
