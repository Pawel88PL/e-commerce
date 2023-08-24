using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Migrations;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly DbStoreContext _context;

        public CheckoutService(DbStoreContext context)
        {
            _context = context;
        }
        
        public async Task<Order?> CreateOrderAsync(Guid sessionId)
        {
            if (_context.ShopingCarts != null)
            {
                var cartItems = await _context.ShopingCarts
                    .Where(cart => cart.ShopingCartId == sessionId)
                    .Include(cart => cart.CartItems)
                    .ThenInclude(item => item.Product)
                    .FirstOrDefaultAsync();

                if (cartItems == null || !cartItems.CartItems.Any())
                {
                    // Brak produktów w koszyku
                    return null;
                }
                else
                {
                    var order = new Order
                    {
                        OrderDate = DateTime.UtcNow,
                        // W zależności od Twojego modelu możesz też dodać inne dane, np. CustomerId
                    };

                    var orderDetails = cartItems.CartItems.Select(item => new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product!.Price  // zakładając, że masz pole Price w modelu Product
                    }).ToList();

                    order.OrderDetails = orderDetails;

                    _context?.Orders?.Add(order);
                    await _context!.SaveChangesAsync();

                    _context.CartItem!.RemoveRange(cartItems.CartItems);
                    await _context.SaveChangesAsync();
                    return order;
                }
            }
            return null;
        }
    }
}