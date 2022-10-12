namespace LearnFast.Web.Controllers
{
    using System.Security.Principal;
    using System.Threading.Tasks;
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
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            model.Owner = user;
            await this.courseService.AddCourse(model);

            return this.Redirect("/");
        }
    }
}
