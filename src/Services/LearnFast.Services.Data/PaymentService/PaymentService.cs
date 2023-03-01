namespace LearnFast.Services.Data.CustomerService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Http;

    using Microsoft.Extensions.Configuration;
    using Stripe;
    using Stripe.Checkout;

    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly IFilterCourse filterCourseService;

        public PaymentService(
            IConfiguration configuration,
            IUserService userService,
            IFilterCourse filterCourseService)
        {
            this.configuration = configuration;
            this.userService = userService;
            this.filterCourseService = filterCourseService;
        }

        public async Task<string> CreateAccountAsync(string refreshUrl, string returnUrl)
        {
            // TODO: should replace with original private key
            StripeConfiguration.ApiKey = this.configuration.GetValue<string>("Stripe:TestPrivateKey");

            var user = await this.userService.GetLoggedUserAsync();

            var service = new AccountService();

            if (user.StripeId != null)
            {
                var account = await service.GetAsync(user.StripeId);
                if (account.PayoutsEnabled)
                {
                    return string.Empty;
                }
            }

            var options = new AccountCreateOptions
            {
                Type = "express",
                Email = user.Email,
            };

            var response = await service.CreateAsync(options);

            user.StripeId = response.Id;
            await this.userService.UpdateAsync(user);

            var linkOptions = new AccountLinkCreateOptions
            {
                Account = response.Id,
                RefreshUrl = refreshUrl,
                ReturnUrl = returnUrl,
                Type = "account_onboarding",
            };

            var linkService = new AccountLinkService();
            var linkResponse = await linkService.CreateAsync(linkOptions);

            return linkResponse.Url;
        }

        public async Task<string> BuyProductAsync(int courseId, string successUrl, string cancelUrl)
        {
            StripeConfiguration.ApiKey = this.configuration.GetValue<string>("Stripe:TestPrivateKey");
            var course = await this.filterCourseService.GetCourseByIdAsync<PaymentCourseModel>(courseId);

            var images = new List<string>();
            images.Add(course.MainImageUrl);

            var productOptions = new ProductCreateOptions
            {
                Name = course.Title,
                Images = images,
            };

            var productService = new ProductService();
            var product = productService.Create(productOptions);

            var priceOptions = new PriceCreateOptions
            {
                UnitAmountDecimal = course.Price,
                Currency = "usd",
                Product = product.Id,
            };

            var priceService = new PriceService();
            var priceResponse = priceService.Create(priceOptions);

            var accountService = new AccountService();
            var seller = accountService.Get(course.Owner.StripeId);

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
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
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
            Session session = await service.CreateAsync(options);

            return session.Url;
        }
    }
}
