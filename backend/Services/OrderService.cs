using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class OrderService : IOrderService
    {
        private readonly DbStoreContext _context;
        private decimal shippingCost = 18.99m;

        public OrderService(DbStoreContext context)
        {
            _context = context;
        }

        public async Task<OrderDTO?> CreateOrderFromCart(Guid cartId, string userId)
        {
            var cart = await _context.ShopingCarts!
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .SingleOrDefaultAsync(c => c.ShopingCartId == cartId);

            if (cart == null || !cart.CartItems.Any())
            {
                return null;
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Nowe",
                TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Price) + shippingCost,
                OrderDetails = cart.CartItems.Select(ci => new OrderDetail
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Price
                }).ToList()
            };

            _context.Orders!.Add(order);
            await _context.SaveChangesAsync();

            _context.CartItem!.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            var orderDto = MapOrderToDto(order);

            return orderDto;

        }

        public OrderDTO? MapOrderToDto(Order order)
        {
            if (order == null) return null;

            var orderDto = new OrderDTO
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Status = order.Status!,
                UserId = order.UserId,
                OrderDetails = order.OrderDetails!.Select(od => new OrderDetailDTO
                {
                    OrderDetailId = od.OrderDetailId,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                }).ToList()
            };

            return orderDto;
        }

    }
}