using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.Core.Services.Contract;
using Stripe;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentIntent(string basketId)
        {
            if(basketId is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var basket=await  paymentService.SetPaymentIntentIdAsync(basketId);

            if (basket == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(basket);
        }

        [HttpPost("Webhook")]
       public async Task<IActionResult> ConfirmPaymentIntent()
       {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            const string endpointSecret = "whsec_0e8a04114f7f2599dd432d33d8b9f742a4fcf544d22b313c6054222d6e8fd1f9";
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json,
                        signatureHeader, endpointSecret);

                var paymentIntent= stripeEvent.Data.Object as PaymentIntent;

                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                  await  paymentService.UpdatePaymentIntentForSucessedOrFailed(paymentIntent.Id,true);

                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {

                    await paymentService.UpdatePaymentIntentForSucessedOrFailed(paymentIntent.Id,false);

                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
