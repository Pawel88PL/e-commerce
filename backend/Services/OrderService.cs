using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class OrderService : IOrderService
    {
        private readonly DbStoreContext _context;
        private readonly IEmailService _emailService;
        private decimal shippingCost = 18.99m;

        public OrderService(DbStoreContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Guid?> CreateOrderFromCart(Guid cartId, string userId)
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
                OrderDate = DateTime.Now,
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

            var orderDetails = await GetOrderDetails(order.OrderId);
            var user = await _context.Users!.SingleOrDefaultAsync(u => u.Id == userId);
            if (user != null && orderDetails != null)
            {
                string userEmail = user.Email ?? "";
                string name = user.Name ?? "";

                await _emailService.SendOrderConfirmationEmail(userEmail, name, orderDetails);
                await _emailService.SendNewOrderNotificationToOwner(orderDetails, user);
            }


            return order.OrderId;
        }

        public async Task<OrderDTO?> GetOrderDetails(Guid orderId)
        {
            var order = await _context.Orders!
                .Include(o => o.OrderDetails!)
                .ThenInclude(od => od.Product)
                .SingleOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return null;

            var orderDto = new OrderDTO
            {
                OrderId = order.OrderId,
                ShortOrderId = order.OrderId.ToString()[..8],
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
                    ProductName = od.Product?.Name ?? string.Empty,
                }).ToList()
            };

            return orderDto;
        }
    }
}