using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
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
            return NotFound( new { message = result.ErrorMessage });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _productService.GetProductAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound( new { message = result.ErrorMessage });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ProductAddDto productAddDto)
        {            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.AddAsync(productAddDto.Product!, productAddDto.ImagePaths!);
            if (!result.Success)
            {
                return StatusCode(500, new { message = result.ErrorMessage });
            }
            return CreatedAtAction(nameof(GetProduct), new { id = result.Data!.ProductId }, result.Data);
        }
    }
}