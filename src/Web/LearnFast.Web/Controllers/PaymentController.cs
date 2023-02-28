namespace LearnFast.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper.Execution;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using NuGet.Protocol.Plugins;
    using Stripe;
    using Stripe.Checkout;
    using Stripe.Issuing;

    public class PaymentController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly IFilterCourse filterCourse;

        public PaymentController(
            IConfiguration configuration,
            IFilterCourse filterCourse)
        {
            this.configuration = configuration;
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
                ReceiptEmail = "georgi1204g@gmail.com",
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

        // create connect account
        public async Task<IActionResult> CreateAccount()
        {
            StripeConfiguration.ApiKey = this.configuration.GetValue<string>("Stripe:TestPrivateKey");

            var options = new AccountCreateOptions
            {
                Type = "express",
                Email = "badrhari@abv.bg",
            };
            var service = new AccountService();
            var response = await service.CreateAsync(options);

            var linkOptions = new AccountLinkCreateOptions
            {
                Account = response.Id,
                RefreshUrl = "https://example.com/reauth", // TODO: redirect course's details page
                ReturnUrl = "https://example.com/return",
                Type = "account_onboarding",
            };

            var linkService = new AccountLinkService();
            var linkResponse = await linkService.CreateAsync(linkOptions);

            return this.Redirect(linkResponse.Url);
        }

        // buy a course
        [Route("/buy")]
        public IActionResult Buy()
        {
            StripeConfiguration.ApiKey = this.configuration.GetValue<string>("Stripe:TestPrivateKey");

            var productOptions = new ProductCreateOptions
            {
                Name = "Gold Special",
            };

            var productService = new ProductService();
            var product = productService.Create(productOptions);

            var priceOptions = new PriceCreateOptions
            {
                UnitAmount = 100000,
                Currency = "bgn",
                Product = product.Id,
            };

            var priceService = new PriceService();
            var priceResponse = priceService.Create(priceOptions);

            var accountService = new AccountService();
            var seller = accountService.Get("acct_1MgUG2R6lE65gAqs");

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                 {
                   new SessionLineItemOptions
                   {
                     Price = priceResponse.Id,
                     Quantity = 1,
                   },
                 },
                Mode = "payment",
                SuccessUrl = "https://example.com/success",
                CancelUrl = "https://example.com/failure",
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    ApplicationFeeAmount = 123,
                    TransferData = new SessionPaymentIntentDataTransferDataOptions
                    {
                        Destination = seller.Id,
                    },
                },
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return this.Redirect(session.Url);
        }
    }
}
