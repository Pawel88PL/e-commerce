using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    public class OrderConfirmationController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderConfirmationController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("orderConfirmation")]
        public async Task<IActionResult> OrderConfirmation([FromForm] ServiceResponse serviceResponse)
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

        [HttpPost("paymentRejected")]
        public async Task<IActionResult> PaymentRejected([FromForm] ServiceResponse serviceResponse)
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

        [HttpPost("orderNotification")]
        public async Task<IActionResult> OrderNotification([FromForm] ServiceResponse serviceResponse)
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
    }
}