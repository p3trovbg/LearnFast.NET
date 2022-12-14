namespace LearnFast.Services.Data
{
    using System.Threading.Tasks;

    using LearnFast.Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

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

        public string GetLoggedUserId()
        {
            return this.userManager.GetUserId(this.httpContext.HttpContext.User);
        }
    }
}
