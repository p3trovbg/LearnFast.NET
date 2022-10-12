namespace LearnFast.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using AutoMapper;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CourseController : BaseController
    {
        private readonly ICourseService courseService;
        private readonly UserManager<ApplicationUser> userManager;

        public CourseController(ICourseService courseService, UserManager<ApplicationUser> userManager)
        {
            this.courseService = courseService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Create(ImportCourseModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            model.Owner = await this.userManager.GetUserAsync(this.User);
            await this.courseService.AddCourse(model);

            return this.Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await this.courseService.DeleteCourseById(id, userId);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return this.Redirect("/");
        }
    }
}
