using Microsoft.AspNetCore.Mvc;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    public class CheckoutController : BaseController
    {
        private readonly ILogger<ICheckoutService> _logger;

        public CheckoutController(
            DbStoreContext context,
            ICartService cartService,
            ICheckoutService checkoutService,
            ICustomerService customerService,
            ILogger<ICheckoutService> logger,
            ITotalCostService totalCostService) :
            base(context, cartService, checkoutService, customerService, totalCostService)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Tunel()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var cartViewModel = await PrepareCartViewModel();
            var model = new CheckoutViewModel
            {
                Customer = await _customerService.GetCustomerAsync(id),
                Cart = cartViewModel
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddNewCustomer()
        {
            var cartViewModel = await PrepareCartViewModel();
            var model = new CheckoutViewModel
            {
                Customer = new Customer(),
                Cart = cartViewModel
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var cartViewModel = await PrepareCartViewModel();
                _logger.LogError("Nie udało się dodać nowego klienta");

                var checkoutViewModel = new CheckoutViewModel
                {
                    Customer = customer,
                    Cart = cartViewModel
                };

                return View(checkoutViewModel);
            }

            await _customerService.AddNewCustomer(customer);
            return RedirectToAction("Index", "Checkout", new { id = customer?.CustomerId });
        }

        [HttpGet]
        public IActionResult OrderConfirmation(int id)
        {
            if (_context.Orders != null)
            {
                var order = _context.Orders.Find(id);
                return View(order);
            }
            return View("_NotFound");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Checkout/PlaceOrderAsync")]
        public async Task<IActionResult> PlaceOrderAsync()
        {
            var sessionId = new Guid(HttpContext.Session.Id);

            var order = await _checkoutService.CreateOrderAsync(sessionId);

            if (order == null)
            {
                _logger.LogError("Nie udało się złożyć zamówienia");
                return View("_NotFound");
            }

            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

    }
}