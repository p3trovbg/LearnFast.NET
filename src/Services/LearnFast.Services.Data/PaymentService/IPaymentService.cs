namespace LearnFast.Services.Data.CustomerService
{
    using System.Threading.Tasks;

    public interface IPaymentService
    {
        public Task<string> CreateCustomerAsync();

        public Task<string> GetCustomerIdentifierByEmailAsync(string email);
    }
}
