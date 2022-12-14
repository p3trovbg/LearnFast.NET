using LearnFast.Data.Models;
using System.Threading.Tasks;

namespace LearnFast.Services.Data
{
    public interface IUserService
    {
        string GetLoggedUserId();

        Task<ApplicationUser> GetLoggedUserAsync();
    }
}
