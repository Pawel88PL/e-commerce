using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class CartService : ICartService
    {
        private readonly DbStoreContext _context;
        private readonly ILogger<CartService> _logger;

        public CartService(DbStoreContext context, ILogger<CartService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ShopingCart?> GetCartAsync(Guid cartId)
        {
            if (_context.ShopingCarts != null)
            {
                if (_context.Products != null)
                {
                    var cart = await _context.ShopingCarts
                        .Include(p => p.CartItems)
                        .ThenInclude(p => p.Product!.ProductImages)
                        .FirstOrDefaultAsync(p => p.ShopingCartId == cartId);

                    return cart;
                }
            }
            return null;
        }

        public async Task AddItemToCart(Guid cartId, int productId, int quantity)
        {
            var cart = await _context.ShopingCarts!
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(p => p.ShopingCartId == cartId);

            if (cart == null)
            {
                cart = new ShopingCart
                {
                    ShopingCartId = cartId,
                    CreateCartDate = DateTime.Now,
                    CartItems = new List<CartItem>()
                };
                _context.ShopingCarts!.Add(cart);
                await _context.SaveChangesAsync();
            }

            var product = await _context.Products!.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                throw new Exception("Nie znaleziono produktu.");
            }

            if (product.AmountAvailable < 1)
            {
                throw new Exception("Niestaty wybrany produkt został sprzedany. Przepraszamy.");
            }

            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    Quantity = quantity,
                    Price = product.Price,
                });
            }

            await _context.SaveChangesAsync();
        }


        public async Task<int> GetCartItemCount(Guid cartId)
        {
            if (_context.ShopingCarts != null)
            {
                var cart = await _context.ShopingCarts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.ShopingCartId == cartId);

                if (cart != null)
                {
                    return cart.CartItems.Sum(item => item.Quantity);
                }
            }
            return 0;
        }

        public async Task<bool> UpdateCartItemQuantityAsync(Guid cartId, int productId, int quantity)
        {
            if (_context.ShopingCarts != null)
            {
                var cart = await _context.ShopingCarts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.ShopingCartId == cartId);

                if (cart != null)
                {
                    var cartItem = cart.CartItems.FirstOrDefault(item => item.ProductId == productId);

                    if (cartItem != null)
                    {
                        cartItem.Quantity = quantity;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                return false;
            }
            return false;

        }


        public async Task<bool> DeleteItemFromCartAsync(Guid cartId, int productId)
        {
            if (_context.ShopingCarts != null)
            {
                var cart = await _context.ShopingCarts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.ShopingCartId == cartId);

                if (cart != null)
                {
                    var productInCart = cart.CartItems.FirstOrDefault(item => item.ProductId == productId);

                    if (productInCart == null)
                    {
                        return false;
                    }

                    if (_context.CartItem != null)
                    {
                        _context.CartItem.Remove(productInCart);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}