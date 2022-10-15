namespace LearnFast.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CourseController : BaseController
    {
        private readonly ICourseService courseService;
        private readonly IFilterCourse filterCourse;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ImageUploadParams imageUpload;

        public CourseController(
            ICourseService courseService,
            UserManager<ApplicationUser> userManager,
            IFilterCourse filterCourse)
        {
            this.courseService = courseService;
            this.userManager = userManager;
            this.filterCourse = filterCourse;
        }

        public IActionResult Index()
        {
            var courses = this.courseService.GetAllAsync<BaseCourseViewModel>();
            var model = new BaseCourseListViewModel { Courses = courses.Result };

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Create(ImportCourseModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            model.Owner = await this.userManager.GetUserAsync(this.User);
            await this.courseService.AddCourseAsync(model);

            return this.Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await this.courseService.DeleteCourseByIdAsync(id, userId);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Update(ImportCourseModel course, int courseId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.courseService.UpdateAsync(course, userId, courseId);
            return this.Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var course = await this.filterCourse.GetByIdAsync<CourseViewModel>(id);
            return this.Redirect("/");
        }
    }
}
