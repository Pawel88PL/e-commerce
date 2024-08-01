using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class OrderConfirmationService : IOrderConfirmationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderConfirmationService> _logger;

        public OrderConfirmationService(
            ApplicationDbContext context,
            IConfiguration configuration,
            IEmailService emailService,
            IOrderService orderService,
            ILogger<OrderConfirmationService> logger)
        {
            _configuration = configuration;
            _context = context;
            _emailService = emailService;
            _logger = logger;
            _orderService = orderService;
        }

        public string GenerateOrderConfirmationURL(int code)
        {
            var successUrl = _configuration["PaymentService:SuccessUrl"]
                ?? throw new ArgumentNullException("RedirectUrl is not set in appsettings.json");

            var failureUrl = _configuration["PaymentService:FailureUrl"]
                ?? throw new ArgumentNullException("RejectUrl is not set in appsettings.json");
            
            var destinationUrl = "";

            if (code == 20 || code == 30 || code == 35)
            {
                destinationUrl = successUrl;
            }
            else if (code == 40 || code == 21 || code == 25)
            {
                destinationUrl = failureUrl;
            }
                        
            var formHtml = $@"
            <html>
            <body onload='document.forms[0].submit()'>
                <form action='{destinationUrl}' method='get'>
                </form>
                <script type='text/javascript'>
                    document.forms[0].submit();
                </script>
            </body>
            </html>";

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

            await SetPaymentStatus(order, serviceResponse.Response_code);
            await SendEmail(order.UserId, order.OrderId);

            return true;
        }

        private async Task SetPaymentStatus(Order order, int? code)
        {
            if (code == 20)
            {
                order.PaymentStatus = "Transakcja zainicjowana";
                await _context.SaveChangesAsync();
            }
            else if (code == 30)
            {
                order.PaymentStatus = "Transakcja zautoryzowana, zakończona";
                order.Status = "Płatność oczekuje na zatwierdzenie do rozliczenia";
                await _context.SaveChangesAsync();
            }
            else if (code == 35)
            {
                order.PaymentStatus = "Transakcja zautoryzowana i zatwierdzona do rozliczenia";
                order.Status = "Opłacone";
                await _context.SaveChangesAsync();
            }
            else if (code == 40)
            {
                order.PaymentStatus = "Transakcja odrzucona przez system autoryzacyjny";
                order.Status = "Odrzucone";
                await _context.SaveChangesAsync();
            }
            else if (code == 21)
            {
                order.PaymentStatus = "Podczas autoryzacji wystąpił błąd techniczny";
                order.Status = "Odrzucone";
                await _context.SaveChangesAsync();
            }
            else if (code == 25)
            {
                order.PaymentStatus = "Weryfikacja nieudana";
                order.Status = "Odrzucone";
                await _context.SaveChangesAsync();
            }
        }


        private async Task SendEmail(string userId, Guid orderId)
        {

            var orderDetails = await _orderService.GetOrderDetails(orderId);

            var user = await _context.Users!.SingleOrDefaultAsync(u => u.Id == userId);
            if (user != null && orderDetails != null)
            {
                string userEmail = user.Email ?? "";
                string name = user.Name ?? "";

                await _emailService.SendOrderConfirmationEmail(userEmail, name, orderDetails);
                //await _emailService.SendNewOrderNotificationToOwner(orderDetails, user);
            }

        }

    }
}