using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    public class BaseController : Controller
    {
        protected readonly DbStoreContext _context;
        protected readonly ICartService _cartService;
        protected readonly ICheckoutService _checkoutService;
        protected readonly ICustomerService _customerService;
        protected readonly ITotalCostService _totalCostService;

        public BaseController(
            DbStoreContext context,
            ICartService cartService,
            ICheckoutService checkoutService,
            ICustomerService customerService,
            ITotalCostService totalCostService)
        {
            _context = context;
            _cartService = cartService;
            _checkoutService = checkoutService;
            _customerService = customerService;
            _totalCostService = totalCostService;
        }

        protected async Task<CartViewModel> PrepareCartViewModel()
        {
            var cartIdStr = HttpContext.Session.GetString("CartId");
            var cartViewModel = new CartViewModel();

            if (!string.IsNullOrEmpty(cartIdStr))
            {
                var cartId = Guid.Parse(cartIdStr);
                var cart = await _cartService.GetCartAsync(cartId);
                var productCost = await _totalCostService.CalculateProductCostAsync(cartId);
                var shippingCost = await _totalCostService.CalculateShippingCostAsync(cartId);
                var totalCost = await _totalCostService.CalculateTotalAsync(cartId);

                cartViewModel = new CartViewModel
                {
                    CartItems = cart?.CartItems ?? new List<CartItem>(),
                    ProductCost = productCost,
                    ShippingCost = shippingCost,
                    TotalCost = totalCost
                };
            }
            return cartViewModel;
        }

        protected async Task<ProductViewModel> PrepareProductViewModel(Product product)
        {
            var productImages = new List<ProductImageInfo>();

            if (_context.ProductImages != null)
            {
                productImages = await _context.ProductImages
                              .Where(pi => pi.ProductId == product.ProductId)
                              .Select(pi => new ProductImageInfo
                              {
                                  ImageId = pi.ImageId,
                                  ImagePath = pi.ImagePath
                              })
                              .ToListAsync();
            }

            return new ProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Priority = product.Priority,
                Description = product.Description,
                Price = product.Price,
                Weight = product.Weight,
                AmountAvailable = product.AmountAvailable,
                CategoryId = product.CategoryId,
                ProductImageInfos = productImages
            };
        }

    }
}