using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("orderConfirmation")]
    public class OrderConfirmation : Controller
    {
        private readonly IOrderService _orderService;

        public OrderConfirmation(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder([FromForm] ServiceResponse serviceResponse)
        {
            if (serviceResponse == null || serviceResponse.Order_id == string.Empty)
            {
                return BadRequest("Nieprawidłowe dane żądania.");
            }

            var isConfirmed = await _orderService.OrderConfirmation(serviceResponse);
            if (!isConfirmed)
            {
                return BadRequest("Wystąpił błąd podczas potwierdzania zamówienia.");
            }

            return Ok();
        }


        [HttpGet]
        public IActionResult TestRedirect()
        {
            var redirectUrl = _orderService.OrderConfirmationTest();
            if (string.IsNullOrEmpty(redirectUrl))
            {
                return BadRequest("Nie udało się utworzyć zamówienia. Koszyk może być pusty lub nie istnieć.");
            }

            return Ok(new { RedirectUrl = redirectUrl });
        }
    }
}