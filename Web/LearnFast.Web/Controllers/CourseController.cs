namespace LearnFast.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Ganss.Xss;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CategoryService;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Services.Data.DifficultyService;
    using LearnFast.Services.Data.LanguageService;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Filter;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CourseController : BaseController
    {
        private const string EmptyView = "Empty";
        private const string CoursesView = "Courses";
        private const string CategoryController = "Category";

        private readonly ICourseService courseService;
        private readonly IFilterCourse filterCourse;
        private readonly ILanguageService languageService;
        private readonly ICategoryService categoryService;
        private readonly IDifficultyService difficultyService;

        private readonly UserManager<ApplicationUser> userManager;

        public CourseController(
            ICourseService courseService,
            UserManager<ApplicationUser> userManager,
            IFilterCourse filterCourse,
            ILanguageService languageService,
            ICategoryService categoryService,
            IDifficultyService difficultyService)
        {
            this.courseService = courseService;
            this.userManager = userManager;
            this.filterCourse = filterCourse;
            this.languageService = languageService;
            this.categoryService = categoryService;
            this.difficultyService = difficultyService;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var model = new ImportCourseModel();

            await this.LoadingBaseParameters(model);

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ImportCourseModel model)
        {
            if (!this.ModelState.IsValid)
            {
                await this.LoadingBaseParameters(model);

                return this.View(model);
            }

            model.Owner = await this.userManager.GetUserAsync(this.User);
            var courseId = await this.courseService.AddCourseAsync(model);

            return this.RedirectToAction(nameof(this.Details), new { id = courseId });
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

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var model = await this.filterCourse.GetByIdAsync<ImportCourseModel>(id);

                if (model.Owner.Id != userId)
                {
                    return this.Unauthorized();
                }

                await this.LoadingBaseParameters(model);

                return this.View(model);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(ImportCourseModel model)
        {
            if (!this.ModelState.IsValid)
            {
                await this.LoadingBaseParameters(model);

                return this.View(model);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.courseService.UpdateAsync(model, userId);

            return this.RedirectToAction(nameof(this.Details), new { id = model.Id });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await this.filterCourse.GetByIdAsync<CourseViewModel>(id);
                model.CurrentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var sanitizer = new HtmlSanitizer();
                model.Description = sanitizer.Sanitize(model.Description);
                model.Requirments = sanitizer.Sanitize(model.Requirments);

                return this.View(model);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Search(SearchViewModel model)
        {
            try
            {
                await this.courseService.GetAllWithFilter(model);
            }
            catch (Exception)
            {
                return this.RedirectToAction(EmptyView, CategoryController);
            }

            return this.View(CoursesView, model);
        }

        private async Task LoadingBaseParameters(ImportCourseModel model)
        {
            model.Languages = await this.languageService.GetLanguageListAsync();
            model.Categories = await this.categoryService.GetCategoryList();
            model.Difficulties = this.difficultyService.GetDifficultyList();
        }
    }
}
