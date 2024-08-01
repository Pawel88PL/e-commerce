using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    public class OrderConfirmationController : Controller
    {

        private readonly IOrderConfirmationService _orderConfirmationService;

        public OrderConfirmationController(IOrderConfirmationService orderConfirmationService)
        {
            _orderConfirmationService = orderConfirmationService;
        }

        [HttpPost("orderConfirmation")]
        public IActionResult OrderConfirmationRedirect([FromForm] ServiceResponse serviceResponse)
        {
            if (serviceResponse == null || serviceResponse.Order_id == string.Empty)
            {
                return BadRequest("Nieprawidłowe dane żądania.");
            }

            var redirectUrl = _orderConfirmationService.GenerateOrderConfirmationURL(serviceResponse.Response_code);
            if (redirectUrl == string.Empty)
            {
                return BadRequest("Wystąpił błąd podczas generowania formularza.");
            }

            return Content(redirectUrl, "text/html");
        }

        [HttpPost("paymentRejected")]
        public IActionResult PaymentRejected([FromForm] ServiceResponse serviceResponse)
        {
            if (serviceResponse == null || serviceResponse.Order_id == string.Empty)
            {
                return BadRequest("Nieprawidłowe dane żądania.");
            }

            var redirectUrl = _orderConfirmationService.GenerateOrderConfirmationURL(serviceResponse.Response_code);
            if (redirectUrl == string.Empty)
            {
                return BadRequest("Wystąpił błąd podczas generowania formularza.");
            }

            return Content(redirectUrl, "text/html");
        }

        [HttpPost("orderNotification")]
        public async Task<IActionResult> OrderNotification([FromForm] ServiceResponse serviceResponse)
        {
            if (serviceResponse == null || serviceResponse.Order_id == string.Empty)
            {
                return BadRequest("Nieprawidłowe dane żądania.");
            }

            var isConfirmed = await _orderConfirmationService.OrderConfirmation(serviceResponse);
            if (!isConfirmed)
            {
                return BadRequest("Wystąpił błąd podczas potwierdzania zamówienia.");
            }

            return Ok();
        }
    }
}