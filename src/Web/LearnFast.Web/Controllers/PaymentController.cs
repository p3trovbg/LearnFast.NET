namespace LearnFast.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Configuration;
    using Stripe;

    public class PaymentController : BaseController
    {
        private readonly IConfiguration configuration;

        public PaymentController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int amount, string payerCustomerId, string recipientCustomerId)
        {
            try
            {
                // Set the API key for Stripe
                StripeConfiguration.ApiKey = "sk_test_1234567890";

                // Create a charge object to represent the payment
                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    Customer = payerCustomerId,
                    TransferData = new ChargeTransferDataOptions
                    {
                        Destination = recipientCustomerId,
                    },
                };

                // Initiate the payment
                var chargeService = new ChargeService();
                Charge charge = await chargeService.CreateAsync(chargeOptions);

                // Payment was successful
                return RedirectToAction("PaymentSuccess");
            }
            catch (StripeException ex)
            {
                // There was an error processing the payment
                return RedirectToAction("PaymentError", new { error = ex.Message });
            }
        }

        public IActionResult PaymentSuccess()
        {
            return this.View();
        }

        public IActionResult PaymentError(string error)
        {
            this.ViewBag.Error = error;
            return this.View();
        }
    }
}
