using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder createOrder)
        {
            if (createOrder == null || createOrder.CartId == Guid.Empty || string.IsNullOrEmpty(createOrder.UserId))
            {
                return BadRequest("Nieprawidłowe dane żądania.");
            }

            var redirectUrl = await _orderService.CreateOrderFromCart(createOrder);
            if (redirectUrl == null)
            {
                return BadRequest("Nie udało się utworzyć zamówienia. Koszyk może być pusty lub nie istnieć.");
            }

            return Content(redirectUrl, "text/html");
        }


        [HttpGet("allOrders")]
        public async Task<IActionResult> GetAllOrders()
        {

            var orders = await _orderService.GetAllOrders();

            return Ok(orders ?? new List<AdminOrderDTO>());
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

        [HttpPost("updateOrderStatus/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] NewOrderStatus newStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _orderService.UpdateOrderStatus(orderId, newStatus.Status);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return NotFound($"Order with ID {orderId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating order status: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }


        public class NewOrderStatus{
            public string Status { get; set; } = string.Empty;
        }
    }
}