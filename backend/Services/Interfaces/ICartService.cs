using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto?> GetCartAsync(Guid ShopingCartId);
        Task AddItemToCart(Guid cartId, int productId, int quantity);
        Task<int> GetCartItemCount(Guid cartId);
        Task<bool> UpdateCartItemQuantityAsync(Guid cartId, int productId, int quantity);
        Task<bool> DeleteItemFromCartAsync(Guid cartId, int productId);
        Task<bool> ClearCartAsync(Guid cartId);
    }
}
