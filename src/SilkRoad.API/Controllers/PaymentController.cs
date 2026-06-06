using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SilkRoad.API;
using SilkRoad.Core;
using Stripe;
using Stripe.V2.Core;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        [HttpPost("create-intent")]
        public async Task<ActionResult<CustomerBasket>> CreateIntent(string basketId, int? deliveryId)
        {
            return await _paymentService.CreateIntentAsync(basketId, deliveryId);
        }
        const string endpointSecret = "whsec_28cc3dec50be3eaba23c0d5217e31f075148d84948bb1e7aa84452952a3a9461";
        [HttpPut("webhooks/stripe")]
        public async Task<IActionResult> UpdateStatusWithStripe()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret, throwOnApiVersionMismatch: false);
                PaymentIntent intent = new PaymentIntent();
                Order order = new Order();
                enStatus? status = null;
                // Handle the event
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        intent = stripeEvent.Data.Object as PaymentIntent ?? new PaymentIntent();
                        status = await _orderService.UpdateOrderStatus(intent.Id,2);
                        break;
                    case "payment_intent.faild":
                        intent = stripeEvent.Data.Object as PaymentIntent ?? new PaymentIntent();
                        status = await _orderService.UpdateOrderStatus(intent.Id,3);
                        break;
                }
                return Ok(new APIResponse(200,status!.Value.ToString()));
            }
            catch (StripeException ex)
            {
                return BadRequest(new APIResponse(200,ex.Message));
            }
        }
    }
}
