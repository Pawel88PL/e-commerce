using Microsoft.AspNetCore.Mvc;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
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

        [HttpGet("{cartId}/items")]
        public async Task<IActionResult> GetCart(Guid cartId)
        {
            try
            {
                var cart = await _cartService.GetCartAsync(cartId);
                if (cart == null)
                {
                    return NotFound("Koszyk nie został znaleziony.");
                }

                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania zawartości koszyka.");
                return StatusCode(500, "Wystąpił błąd serwera.");
            }
        }

        [HttpPut("{cartId}/items/{productId}")]
        public async Task<IActionResult> UpdateCartItemQuantity(Guid cartId, int productId, [FromBody] int quantity)
        {
            try
            {
                var result = await _cartService.UpdateCartItemQuantityAsync(cartId, productId, quantity);
                if (!result)
                {
                    return NotFound("Produkt lub koszyk nie został znaleziony.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas aktualizacji ilości produktu.");
                return StatusCode(500, "Wystąpił błąd serwera.");
            }
        }

        [HttpDelete("{cartId}/items/{productId}")]
        public async Task<IActionResult> DeleteItemFromCart(Guid cartId, int productId)
        {
            try
            {
                var result = await _cartService.DeleteItemFromCartAsync(cartId, productId);
                if (!result)
                {
                    return NotFound("Produkt lub koszyk nie został znaleziony.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas usuwania produktu z koszyka.");
                return StatusCode(500, "Wystąpił błąd serwera.");
            }
        }

        [HttpDelete("{cartId}/clear")]
        public async Task<IActionResult> ClearCart(Guid cartId)
        {
            try
            {
                var result = await _cartService.ClearCartAsync(cartId);
                if (!result)
                {
                    return NotFound("Koszyk nie został znaleziony.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas czyszczenia koszyka.");
                return StatusCode(500, "Wystąpił błąd serwera.");
            }
        }
    }
}
