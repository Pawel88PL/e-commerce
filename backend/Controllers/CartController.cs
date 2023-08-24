using Microsoft.AspNetCore.Mvc;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MiodOdStaniula.Controllers
{
    public class CartController : BaseController
    {
        private readonly ILogger<ICartService> _logger;

        public CartController(
            DbStoreContext context,
            ICartService cartService,
            ICheckoutService checkoutService,
            ICustomerService customerService,
            ILogger<ICartService> logger,
            ITotalCostService totalCostService) :
            base(context, cartService, checkoutService, customerService, totalCostService)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cartViewModel = await PrepareCartViewModel();
            return View(cartViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItemCount()
        {
            var cartIdStr = HttpContext.Session.GetString("CartId");
            int itemCount = 0;

            if (!string.IsNullOrEmpty(cartIdStr))
            {
                var cartId = Guid.Parse(cartIdStr);
                itemCount = await _cartService.GetCartItemCount(cartId);
            }
            return Ok(itemCount);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart(AddCartItemModel model)
        {
            try
            {
                var cartIdStr = HttpContext.Session.GetString("CartId");

                if (string.IsNullOrEmpty(cartIdStr))
                {
                    var newCart = new ShopingCart();
                    if (_context.ShopingCarts != null)
                    {
                        newCart.CreateCartDate = DateTime.Now;
                        _context.ShopingCarts.Add(newCart);
                        await _context.SaveChangesAsync();

                        cartIdStr = newCart.ShopingCartId.ToString();

                        HttpContext.Session.SetString("CartId", cartIdStr);
                    }
                }

                if (cartIdStr != null)
                {
                    var cartId = Guid.Parse(cartIdStr);
                    await _cartService.AddItemToCart(cartId, model.ProductId, model.Quantity);

                    if (_context.Products != null)
                    {
                        var product = await _context.Products
                            .Include(p => p.ProductImages)
                            .FirstOrDefaultAsync(p => p.ProductId == model.ProductId);

                        var firstImagePath = product?.ProductImages?.FirstOrDefault()?.ImagePath;

                        if (product?.AmountAvailable > 0)
                        {
                            return Json(new
                            {
                                success = true,
                                product = new
                                {
                                    name = product?.Name,
                                    image = firstImagePath,
                                    weight = product?.Weight,
                                    price = product?.Price.ToString("C2"),
                                }
                            });
                        }
                        else
                        {
                            return Json(new { success = false });
                        }
                    }
                    return Json(new { success = false });
                }
                _logger.LogError("Nie udało się dodać produktu do koszyka");
                return View("_NotFound");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Pojawił się błąd podczas dodawania produktu do koszyka.\nSpróbuj ponownie później.";
                return RedirectToAction("Index", "Products");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Cart/UpdateCartItemQuantity")]
        public async Task<IActionResult> UpdateCartItemQuantity(int productId, int quantity)
        {
            var cartIdStr = HttpContext.Session.GetString("CartId");
            if (string.IsNullOrEmpty(cartIdStr))
            {
                return View("_NotFound");
            }
            var cartId = Guid.Parse(cartIdStr);
            var result = await _cartService.UpdateCartItemQuantityAsync(cartId, productId, quantity);
            if (!result)
            {
                _logger.LogError("Nie udało się zaktualizować ilości produktu w koszyku");
                return View("_NotFound");
            }
            return RedirectToAction("Index", "Cart");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Cart/DeleteItemFromCartAsync")]
        public async Task<IActionResult> DeleteItemFromCartAsync([FromForm] int productId)
        {
            var cartIdStr = HttpContext.Session.GetString("CartId");
            if (string.IsNullOrEmpty(cartIdStr))
            {
                return View("_NotFound");
            }
            var cartId = Guid.Parse(cartIdStr);
            var result = await _cartService.DeleteItemFromCartAsync(cartId, productId);

            if (!result)
            {
                _logger.LogError("Nie udało się usunąć produktu z koszyka");
                return View("_NotFound");
            }

            return RedirectToAction("Index", "Cart");
        }
    }
}
