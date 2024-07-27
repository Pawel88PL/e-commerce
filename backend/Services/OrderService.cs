using System.Globalization;
using System.Web;
using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<OrderService> _logger;
        private readonly IPaymentService _paymentService;
        private decimal _shippingCost;

        public OrderService(
            ApplicationDbContext context,
            IEmailService emailService,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<OrderService> logger,
            IPaymentService paymentService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _paymentService = paymentService;
            _shippingCost = decimal.Parse(_configuration["ApplicationSettings:ShippingCost"]!, CultureInfo.InvariantCulture);

        }

        public async Task<string?> CreateOrderFromCart(CreateOrder createOrder)
        {
            var cart = await _context.ShopingCarts!
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .SingleOrDefaultAsync(c => c.ShopingCartId == createOrder.CartId);

            if (cart == null || !cart.CartItems.Any() || createOrder.UserId == null)
            {
                return null;
            }

            if (createOrder.IsPickupInStore)
            {
                _shippingCost = 0;
            }

            var order = new Order
            {
                UserId = createOrder.UserId,
                OrderDate = DateTime.Now,
                Status = "Oczekuje na płatność",
                IsPickupInStore = createOrder.IsPickupInStore,
                TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Price) + _shippingCost,
                OrderDetails = cart.CartItems.Select(ci => new OrderDetail
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Price
                }).ToList()
            };

            _context.Orders!.Add(order);
            _context.CartItem!.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            var formHtml = _paymentService.ProcessPayment(order.OrderId, order.TotalPrice, order.UserId);
            return formHtml;
        }

        public async Task<bool> OrderConfirmation(ServiceResponse serviceResponse)
        {
            if (serviceResponse == null || serviceResponse.Order_id == null)
            {
                _logger.LogError("Invalid request data. ServiceResponse: {ServiceResponse}", serviceResponse);
                return false;
            }

            var order = await _context.Orders!.FindAsync(Guid.Parse(serviceResponse.Order_id));

            if (order == null)
            {
                _logger.LogError("Order not found. OrderId: {OrderId}", serviceResponse.Order_id);
                return false;
            }

            order.TransactionId = serviceResponse.Transaction_id;

            if (serviceResponse.Response_code == 20)
            {
                order.PaymentStatus = "Transakcja zainicjowana";
                await _context.SaveChangesAsync();
            }
            else if (serviceResponse.Response_code == 30)
            {
                order.PaymentStatus = "Transakcja zautoryzowana, zakończona";
                order.Status = "Płatność oczekuje na zatwierdzenie do rozliczenia";
                await _context.SaveChangesAsync();
            }
            else if (serviceResponse.Response_code == 35)
            {
                order.PaymentStatus = "Transakcja zautoryzowana i zatwierdzona do rozliczenia";
                order.Status = "Opłacone";
                await _context.SaveChangesAsync();
            }

            await SendEmail(order.UserId, order.OrderId);

            return true;
        }

        public string OrderConfirmationTest()
        {
            var redirectUrl = "https://miododstaniula.pl/orderConfirmation";

            return redirectUrl;
        }


        public async Task<List<AdminOrderDTO>> GetAllOrders()
        {
            var orders = await _context.Orders!
                .Include(u => u.User)
                .Select(order => new AdminOrderDTO
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    IsPickupInStore = order.IsPickupInStore,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status ?? string.Empty,
                    CustomerName = order.User.Name + " " + order.User.Surname ?? string.Empty
                })
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders;
        }

        public async Task<OrderDTO?> GetOrderDetails(Guid orderId)
        {
            var order = await _context.Orders!
                .Include(u => u.User)
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

                Customer = order.User! != null ? new UserDto
                {
                    Name = order.User.Name,
                    Email = order.User.Email ?? string.Empty,
                    Surname = order.User.Surname,
                    City = order.User.City ?? string.Empty,
                    Street = order.User.Street ?? string.Empty,
                    Address = order.User.Address ?? string.Empty,
                    PostalCode = order.User.PostalCode ?? string.Empty,
                    PhoneNumber = order.User.PhoneNumber ?? string.Empty
                } : null!,
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

        private async Task SendEmail(string userId, Guid orderId)
        {

            var orderDetails = await GetOrderDetails(orderId);

            var user = await _context.Users!.SingleOrDefaultAsync(u => u.Id == userId);
            if (user != null && orderDetails != null)
            {
                string userEmail = user.Email ?? "";
                string name = user.Name ?? "";

                await _emailService.SendOrderConfirmationEmail(userEmail, name, orderDetails);
                //await _emailService.SendNewOrderNotificationToOwner(orderDetails, user);
            }

        }

        public async Task<bool> UpdateOrderStatus(Guid orderId, string newStatus)
        {
            var order = await _context.Orders!.FindAsync(orderId);
            if (order == null)
            {
                return false;
            }

            order.Status = newStatus;

            try
            {
                await _context.SaveChangesAsync();

                if (newStatus != "Zrealizowane")
                {
                    var userId = order.UserId;
                    var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
                    var orderDetails = await GetOrderDetails(order.OrderId);
                    if (user != null && orderDetails != null)
                    {
                        string userEmail = user.Email ?? "";
                        string name = user.Name ?? "";

                        await _emailService.SendOrderStatusChangeEmail(userEmail, name, orderDetails);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the order status. OrderId: {OrderId}, NewStatus: {NewStatus}", orderId, newStatus);
                throw;
            }
        }

    }
}