namespace LearnFast.Services.Data.CustomerService
{
    using System;
    using System.Threading.Tasks;

    public class CustomerService : ICustomerService
    {
        public Task<string> CreateCustomerAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetCustomerByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
