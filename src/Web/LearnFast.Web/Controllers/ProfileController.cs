namespace LearnFast.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using LearnFast.Common;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.ApplicationUser;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class ProfileController : BaseController
    {
        private readonly IUserService userService;

        public ProfileController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Index(string username)
        {
            try
            {
                var user = await this.userService.GetAllUsersAsQueryable()
                    .Where(x => x.UserName == username)
                    .To<UserViewModel>()
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new ArgumentException(GlobalExceptions.UserNotExists);
                }

                var currentUserId = await this.userService.GetLoggedUserIdAsync();
                if (currentUserId == user.Id)
                {
                    user.IsOwner = true;
                }

                return this.View(user);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        public async Task<IActionResult> AddOwnSite(string url)
        {
            var user = await this.userService.GetLoggedUserAsync();
            user.WebsitePath = url;

            await this.userService.UpdateAsync(user);

            return this.RedirectToAction(nameof(this.Index), new { username = user.UserName });
        }
    }
}
