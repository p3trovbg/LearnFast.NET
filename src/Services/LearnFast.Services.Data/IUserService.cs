namespace LearnFast.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LearnFast.Data.Models;

    public interface IUserService
    {
        Task<string> GetLoggedUserIdAsync();

        Task<ApplicationUser> GetLoggedUserAsync();

        IQueryable<ApplicationUser> GetAllUsersAsQueryable();

        Task<IEnumerable<T>> GetAllUsersAsync<T>();

        Task UpdateAsync(ApplicationUser user);
    }
}
