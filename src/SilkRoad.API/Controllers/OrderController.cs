using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SilkRoad.API;
using SilkRoad.Core;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("delivery-methodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> GetDeliveryMethodsAsync(string? searchTerm)
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodsAsync(searchTerm);
            return Ok(deliveryMethods);
        }

        [HttpGet("order/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderByIdAsync(Guid Id)
        {
            if (Id == Guid.Empty)
                return BadRequest(new APIResponse(400));
            string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new APIResponse(401));
            }
            var order = await _orderService.GetOrderByIdAsync(Id, userId);
            if (order is null)
                return NotFound(new APIResponse(404));
            return Ok(order);
        }

        [HttpGet("user-orders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetUserOrdersAsync()
        {
            string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new APIResponse(401));
            }

            var orders = await _orderService.GetUserOrdersAsync(userId);

            if (orders is null || orders.Count == 0)
                return NotFound(new APIResponse(404));
            return Ok(orders);
        }


        [HttpPost("place-order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PlaseOrderAsync(PlaceOrderDTO orderDTO)
        {
            string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new APIResponse(401));
            }

            await _orderService.PlaceOrderAsync(orderDTO, userId);
            return Ok(new APIResponse(200));
        }
    }
}
