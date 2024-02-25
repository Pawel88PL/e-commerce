using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class OrderService : IOrderService
    {
        private readonly DbStoreContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private decimal _shippingCost;

        public OrderService(DbStoreContext context, IEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
            _shippingCost = decimal.Parse(_configuration["ApplicationSettings:ShippingCost"]!, CultureInfo.InvariantCulture);

        }

        public async Task<Guid?> CreateOrderFromCart(Guid cartId, string userId, bool isPickupInStore)
        {
            var cart = await _context.ShopingCarts!
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .SingleOrDefaultAsync(c => c.ShopingCartId == cartId);

            if (cart == null || !cart.CartItems.Any())
            {
                return null;
            }

            if (isPickupInStore)
            {
                _shippingCost = 0;
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = "Oczekuje na płatność",
                IsPickupInStore = isPickupInStore,
                TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Price) + _shippingCost,
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
                IsPickupInStore = order.IsPickupInStore,
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

        public async Task<List<OrderHistoryDTO>> GetOrdersHistory(string userId)
        {
            var orders = await _context.Orders!
                .Where(o => o.UserId == userId)
                .Select(order => new OrderHistoryDTO
                {
                    OrderId = order.OrderId.ToString(),
                    OrderDate = order.OrderDate,
                    IsPickupInStore = order.IsPickupInStore,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status!
                })
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders;
        }
    }
}