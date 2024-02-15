using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class OrderService : IOrderService
    {
        private readonly DbStoreContext _context;

        public OrderService(DbStoreContext context)
        {
            _context = context;
        }

        public async Task<Order?> CreateOrderFromCart(Guid cartId, string userId)
        {
            // Sprawdź, czy koszyk istnieje i ma pozycje
            var cart = await _context.ShopingCarts!
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .SingleOrDefaultAsync(c => c.ShopingCartId == cartId);

            if (cart == null || !cart.CartItems.Any())
            {
                return null; // Nie ma takiego koszyka lub jest pusty
            }

            // Utwórz nowe zamówienie
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Nowe", // Możesz dostosować statusy zamówień zgodnie z potrzebami aplikacji
                TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Price), // Oblicz całkowitą cenę
                OrderDetails = cart.CartItems.Select(ci => new OrderDetail
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Price
                }).ToList()
            };

            _context.Orders!.Add(order);
            await _context.SaveChangesAsync();

            // Opcjonalnie: Możesz chcieć usunąć pozycje z koszyka po utworzeniu zamówienia
            _context.CartItem!.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return order;
        }

    }
}