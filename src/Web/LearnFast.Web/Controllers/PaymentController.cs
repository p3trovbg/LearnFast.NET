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
        private readonly IPaymentService paymentService;
        private readonly IFilterCourse filterCourse;

        public PaymentController(
            IConfiguration configuration,
            IPaymentService paymentService,
            IFilterCourse filterCourse)
        {
            this.configuration = configuration;
            this.paymentService = paymentService;
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
            StripeConfiguration.ApiKey = this.configuration.GetValue<string>("Stripe:TestPrivateKey");

            var charge = await new ChargeService().CreateAsync(new ChargeCreateOptions
            {
                Amount = (long)(100 * 100), // Convert amount to cents
                Currency = "usd",
                Description = "MyApplication Payment",
                Source = stripeToken,
                ReceiptEmail = stripeEmail,
            });

            if (charge.Status == "succeeded")
            {
                return this.View("Success");
            }
            else
            {
                return this.View("Error");
            }
        }
    }
}
