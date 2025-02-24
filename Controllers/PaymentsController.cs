using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.IO;
using System.Threading.Tasks;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Microsoft.Extensions.Logging;
using Talabat.Core.Entities.Order_Aggregate;
using Stripe;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;

namespace Talabat.APIs.Controllers
{
    
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _whSecret = "whsec_9f80a4f71d0a79295d01f98c2e63dfeff8a54b8a23c7f09db799a6bd7fae294d";
        public PaymentsController(IPaymentService paymentService
            ,ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null) return BadRequest(new ApiResponse(400,"a problem with your basket"));


            return Ok(basket);
        }
        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Strips-Signature"], _whSecret);

                PaymentIntent intent;
                Order order;
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        intent = (PaymentIntent) stripeEvent.Data.Object;
                        order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(intent.Id, true);
                        _logger.LogInformation("Payment Is Succeeded",order.Id,intent.Id);
                        break;
                    case "payment_intent.payment_failed":
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(intent.Id,false);
                        _logger.LogInformation("Payment Is Faild",order.Id,intent.Id);
                        break;
                }
            return new EmptyResult();
           
        }

    }
}
