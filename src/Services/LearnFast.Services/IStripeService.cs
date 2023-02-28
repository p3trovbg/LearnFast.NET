using System.Threading.Tasks;

namespace LearnFast.Services
{
    public interface IStripeService
    {
        Task<string> CreateConnectAccountAsync();
    }
}
