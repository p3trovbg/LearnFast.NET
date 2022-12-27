namespace LearnFast.Web.Controllers
{
    using System.Threading.Tasks;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Services.Data.CustomerService;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Stripe;

    public class PaymentController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly IPaymentCustomerService paymentCustomerService;
        private readonly IFilterCourse filterCourse;

        public PaymentController(
            IConfiguration configuration,
            IPaymentCustomerService paymentCustomerService,
            IFilterCourse filterCourse)
        {
            this.configuration = configuration;
            this.paymentCustomerService = paymentCustomerService;
            this.filterCourse = filterCourse;
        }

        [Route("/Payment")]
        public IActionResult Payment()
        {
            var key = this.configuration.GetValue<string>("Stripe:PublishKey");
            this.ViewBag.Key = key;
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Charge(decimal amount, string stripeEmail, string stripeToken)
        {
            // Configure Stripe API keys
            //StripeConfiguration.ApiKey = this.configuration.GetValue<string>("Stripe:PrivateKey");

            StripeConfiguration.ApiKey = this.configuration.GetValue<string>("Stripe:TestPrivateKey");

            // Create the charge on Stripe's servers - this will charge the user's card
            var charge = await new ChargeService().CreateAsync(new ChargeCreateOptions
            {
                Amount = (long)(100 * 100), // Convert amount to cents
                Currency = "usd",
                Description = "MyApplication Payment",
                Source = stripeToken,
                ReceiptEmail = stripeEmail,
            });

            // Check if the charge was successful
            if (charge.Status == "succeeded")
            {
                // Payment was successful
                return this.View("Success");
            }
            else
            {
                // Payment failed
                return this.View("Error");
            }
        }
    }
}
