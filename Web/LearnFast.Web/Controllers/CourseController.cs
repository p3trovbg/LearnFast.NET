namespace LearnFast.Web.Controllers
{
    using System;
    using System.Security.Claims;
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
        private readonly ILanguageService languageService;
        private readonly ICategoryService categoryService;
        private readonly IFilterCourse filterCourse;
        private readonly UserManager<ApplicationUser> userManager;

        public CourseController(
            ICourseService courseService,
            UserManager<ApplicationUser> userManager,
            ILanguageService languageService,
            ICategoryService categoryService,
            IFilterCourse filterCourse)
        {
            this.courseService = courseService;
            this.userManager = userManager;
            this.languageService = languageService;
            this.categoryService = categoryService;
            this.filterCourse = filterCourse;
        }

        public IActionResult Index()
        {
            var courses = this.courseService.GetAllAsync<BaseCourseViewModel>();
            // TODO 
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
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.courseService.UpdateAsync(course, userId, courseId);
            return this.Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Current(int id)
        {
            var course = await this.filterCourse.GetByIdAsync(id);
            ;
            return this.Redirect("/");
        }
    }
}
