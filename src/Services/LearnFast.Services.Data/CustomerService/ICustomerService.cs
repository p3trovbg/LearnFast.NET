namespace LearnFast.Services.Data.CustomerService
{
    using System.Threading.Tasks;

    public interface ICustomerService
    {
        public Task<string> CreateCustomerAsync(string email);

        public Task<string> GetCustomerByEmailAsync(string email);
    }
}
