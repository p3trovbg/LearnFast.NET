namespace LearnFast.Services.Data.CustomerService
{
    using System.Threading.Tasks;

    public interface IPaymentService
    {
        public Task<string> CreateAccountAsync(string refreshUrl, string returnUrl);

        public Task<string> BuyProductAsync(int courseId, string successUrl, string cancelUrl);
    }
}
