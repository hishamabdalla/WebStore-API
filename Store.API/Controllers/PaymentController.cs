using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.Core.Services.Contract;

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
        [Authorize]
       public async Task<IActionResult> ConfirmPaymentIntent()
        {
            return Ok();
        }


    }
}
