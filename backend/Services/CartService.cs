using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartService> _logger;

        public CartService(ApplicationDbContext context, ILogger<CartService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CartDto?> GetCartAsync(Guid cartId)
        {
            var cart = await _context.ShopingCarts!
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p!.ProductImages)
                .FirstOrDefaultAsync(c => c.ShopingCartId == cartId);

            if (cart == null)
                return null;

            var cartDto = new CartDto
            {
                ShopingCartId = cart.ShopingCartId,
                CartItems = cart.CartItems.Select(i => new CartItemDto
                {
                    AmountAvailable = i.Product!.AmountAvailable,
                    ProductId = i.ProductId,
                    Name = i.Product!.Name!,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ImagePaths = i.Product!.ProductImages!.Select(img => img!.ImagePath!).ToList()
                }).ToList(),
                TotalValue = cart.GetTotalValue()
            };

            return cartDto;
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

        public async Task AssignCartToUser(Guid cartId, Guid userId)
        {
            var cart = await _context.ShopingCarts!.FindAsync(cartId);
            if (cart != null)
            {
                cart.CustomerId = userId;
                await _context.SaveChangesAsync();
            }
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

        public async Task<bool> ClearCartAsync(Guid cartId)
        {
            var cart = await _context.ShopingCarts!
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.ShopingCartId == cartId);

            if (cart == null)
            {
                return false;
            }

            _context.CartItem!.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}