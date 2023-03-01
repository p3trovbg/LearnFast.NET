namespace LearnFast.Services.Data.CustomerService
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Stripe;

    public class PaymentService : IPaymentService
    {

        private readonly IConfiguration configuration;
        private readonly IDeletableEntityRepository<PaymentCustomer> repository;
        private readonly IUserService userService;

        public PaymentService(
            IConfiguration configuration,
            IDeletableEntityRepository<PaymentCustomer> repository,
            IUserService userService)
        {
            this.configuration = configuration;
            this.repository = repository;
            this.userService = userService;
        }

        public Task<string> CreateCustomerAsync()
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

        public Task<string> GetCustomerIdentifierByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
