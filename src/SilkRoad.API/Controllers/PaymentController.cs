using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SilkRoad.Core;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-or-update")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePayment(string basketId, int? deliveryId)
        {
            return await _paymentService.CreateOrUpdatePaymentAsync(basketId,deliveryId);
        }
    }
}
