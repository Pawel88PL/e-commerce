using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IAddNewProductService _addNewProductService;
        private readonly IProductService _productService;

        public ProductsController(IAddNewProductService addNewProductService, IProductService productService)
        {
            _addNewProductService = addNewProductService;
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
            var result = await _productService.GetProductAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, new { message = result.ErrorMessage });
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductAddDto productAddDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Nieprawidłowe dane produktu" });
            }

            var result = await _addNewProductService.AddNewProductAsync(productAddDto.Product!, productAddDto.ImagePaths!);

            if (!result.Success)
            {
                return StatusCode(500, new { message = result.ErrorMessage });
            }

            return Ok(new { message = "Produkt został pomyślnie dodany", product = result.Data });
        }
    }
}
