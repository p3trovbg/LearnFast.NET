namespace LearnFast.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;
    using LearnFast.Common;
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.ApplicationUser;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class ProfileController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileController(
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string username)
        {
            try
            {
                var user = await this.userManager.Users
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => x.UserName == username)
                    .To<UserViewModel>()
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new ArgumentException(GlobalExceptions.UserNotExists);
                }

                var currentUserId = this.userManager.GetUserId(this.User);
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
            var user = await this.userManager.GetUserAsync(this.User);
            user.WebsitePath = url;

            await this.userManager.UpdateAsync(user);

            return this.RedirectToAction(nameof(this.Index), new { username = user.UserName });
        }

        public async Task<IActionResult> EnrollingCourse(int courseId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            user.BuyedCourses.Add(new StudentCourse { User = user, CourseId = courseId });

            await this.userManager.UpdateAsync(user);

            return this.RedirectToAction(nameof(this.Index), new { username = user.UserName });
        }
    }
}
