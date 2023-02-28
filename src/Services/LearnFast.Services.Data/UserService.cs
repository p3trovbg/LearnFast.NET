namespace LearnFast.Services.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContext;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContext)
        {
            this.userManager = userManager;
            this.httpContext = httpContext;
        }

        public async Task<ApplicationUser> GetLoggedUserAsync()
        {
            return await this.userManager.GetUserAsync(this.httpContext.HttpContext.User);
        }

        public async Task<string> GetLoggedUserIdAsync()
        {
            var user = await this.userManager.GetUserAsync(this.httpContext.HttpContext.User);
            return user.Id;
        }

        public async Task<IEnumerable<T>> GetAllUsersAsync<T>()
        {
           return await this.userManager.Users
                .AsNoTrackingWithIdentityResolution()
                .To<T>()
                .ToListAsync();
        }

        public IQueryable<ApplicationUser> GetAllUsersAsQueryable()
        {
            return this.userManager.Users
                 .AsNoTrackingWithIdentityResolution();
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            await this.userManager.UpdateAsync(user);
        }
    }
}
