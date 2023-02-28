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
        {
            throw new NotImplementedException();
        }

        public Task<string> GetCustomerIdentifierByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
