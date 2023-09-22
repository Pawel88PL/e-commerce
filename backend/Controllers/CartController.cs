using Microsoft.AspNetCore.Mvc;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddCartItem request)
        {
            try
            {
                await _cartService.AddItemToCart(request.CartId, request.ProductId, request.Quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Nie znaleziono koszyka.") || ex.Message.Contains("Nie znaleziono produktu."))
                {
                    return NotFound(ex.Message);
                }
                if (ex.Message.Contains("Niestety brak produktu w magazynie."))
                {
                    return BadRequest(ex.Message);
                }
                return StatusCode(500, "Wystąpił błąd podczas dodawania produktu do koszyka.");
            }
        }
    }
}
