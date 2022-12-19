namespace LearnFast.Services.Data
{
    using System.Threading.Tasks;

    using LearnFast.Data.Models;

    public interface IUserService
    {
        string GetLoggedUserId();

        Task<ApplicationUser> GetLoggedUserAsync();
    }
}
