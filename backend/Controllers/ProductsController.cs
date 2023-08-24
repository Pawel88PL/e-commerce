using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProductsAsync();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, new { message = result.ErrorMessage });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            Product? product = await _warehouseService.GetProductAsync(id)!;

            if (product == null)
            {
                return NotFound(new { Message = "Product not found." });
            }
            else
            {
                ProductViewModel productViewModel = await PrepareProductViewModel(product);
                return Ok(productViewModel);
            }
        }

    }
}
