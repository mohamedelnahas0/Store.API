using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BasketService.Dots;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;
using Stripe;

namespace Store.API.Controllers
{

    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentservice;
        private readonly ILogger<PaymentController> _logger;
        private const string endpointSecret = "whsec_72ce80d3ef5d2df5a5296621141c89dd979c6fcf97342f0dc2902834a2396c1";


        public PaymentController(IPaymentService paymentservice, ILogger<PaymentController> logger)
        {
            _paymentservice = paymentservice;
            _logger = logger;
        }
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto input)
       => Ok(await _paymentservice.CreateOrUpdatePaymentIntentForExistingOrder(input));

        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForNewOrder(string basketId)
      => Ok(await _paymentservice.CreateOrUpdatePaymentIntentForNewOrder(basketId));


        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);

                PaymentIntent paymentIntent;
                OrderResultDto order;

                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed win", paymentIntent.Id);
                    order = await _paymentservice.UpdateOrderPaymentFaild(paymentIntent.Id);
                    _logger.LogInformation("Order Updated To Payment Failed ", order.Id);

                }

                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeed:", paymentIntent.Id);
                    order = await _paymentservice.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    _logger.LogInformation("Order Updated To PaymentSucceeded ", order.Id);
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                else
                {
                    Console.WriteLine("Unhanded eventtype: {0}" , stripeEvent.Type);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
    }
    }
}
