using Microsoft.AspNetCore.Mvc;
using MiodOdStaniula.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MiodOdStaniula.Models;

namespace MiodOdStaniula.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request == null || request.CartId == Guid.Empty || string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest("Nieprawidłowe dane żądania.");
            }

            var order = await _orderService.CreateOrderFromCart(request.CartId, request.UserId);
            if (order == null)
            {
                return BadRequest("Nie udało się utworzyć zamówienia. Koszyk może być pusty lub nie istnieć.");
            }

            return Ok(order);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetails(Guid orderId)
        {
            var orderDetails = await _orderService.GetOrderDetails(orderId);

            if (orderDetails == null)
            {
                return NotFound("Nie znaleziono zamówienia o podanym identyfikatorze.");
            }

            return Ok(orderDetails);
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetOrdersHistory([FromRoute] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("Nieprawidłowy identyfikator użytkownika.");
            }

            var ordersHistory = await _orderService.GetOrdersHistory(userId.ToString());

            return Ok(ordersHistory ?? new List<OrderHistoryDTO>());
        }


        public class CreateOrderRequest
        {
            public Guid CartId { get; set; }
            public string UserId { get; set; } = string.Empty;
        }
    }
}