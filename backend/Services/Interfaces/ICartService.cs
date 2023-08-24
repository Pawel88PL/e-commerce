using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface ICartService
    {
        Task<ShopingCart?> GetCartAsync(Guid ShopingCartId);
        Task AddItemToCart(Guid cartId, int productId, int quantity);
        Task<int> GetCartItemCount(Guid cartId);
        Task<bool> UpdateCartItemQuantityAsync(Guid cartId, int productId, int quantity);
        Task<bool> DeleteItemFromCartAsync(Guid cartId, int productId);
    }
}
