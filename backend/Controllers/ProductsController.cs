using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IWarehouseService _warehouseService;
        private readonly IProductService _productService;

        public ProductsController(
            DbStoreContext context,
                ICartService cartService,
                ICheckoutService checkoutService,
                ICustomerService customerService,
                IProductService productService,
                ITotalCostService totalCostService,
                ILogger<IEditProductService> logger,
                IWarehouseService warehouseService
            ) : base(context, cartService, checkoutService, customerService, totalCostService)
        {
            _warehouseService = warehouseService;
            _productService = productService;
        }

        private List<ProductViewModel> ConvertToViewModel(IEnumerable<Product> products)
        {
            
            return products.Select(product => new ProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Weight = product.Weight,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Priority = product.Priority,
                AmountAvailable = product.AmountAvailable,
                ProductImageInfos = product.ProductImages?.Select(pi => new ProductImageInfo
                {
                    ImageId = pi.ImageId,
                    ImagePath = pi.ImagePath
                }).ToList() ?? new List<ProductImageInfo>()

            }).ToList();
        }


        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string filterCondition)
        {
            var products = await GetSortedAndFilteredProducts(sortOrder, filterCondition);
            var viewModel = ConvertToViewModel(products);
            ViewBag.FilterCondition = string.IsNullOrEmpty(filterCondition) ? "NASZE PRODUKTY:" : filterCondition;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Product? product = await _warehouseService.GetProductAsync(id)!;
            var cartId = HttpContext.Session.GetString("CartId");

            ViewData["CartId"] = cartId;

            if (product == null)
            {
                return View("_NotFound");
            }
            else
            {
                ProductViewModel productViewModel = await PrepareProductViewModel(product);
                return View(productViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string sortOrder, string filterCondition)
        {
            var products = await GetSortedAndFilteredProducts(sortOrder, filterCondition);
            var viewModel = ConvertToViewModel(products);
            ViewBag.FilterCondition = filterCondition;
            return PartialView("_ProductList", viewModel);
        }

        private async Task<IEnumerable<Product>> GetSortedAndFilteredProducts(string sortOrder, string filterCondition)
        {
            var products = await _warehouseService.GetAllProductsAsync();
            products = _productService.Sort(products, sortOrder);
            products = _productService.Filter(products, filterCondition);
            return products;
        }
    }
}
