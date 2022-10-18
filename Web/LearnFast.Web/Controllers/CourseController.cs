namespace LearnFast.Web.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CloudinaryDotNet.Actions;
    using LearnFast.Data.Models;
    using LearnFast.Data.Models.Enums;
    using LearnFast.Services.Data;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Language;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CourseController : BaseController
    {
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

        public IActionResult Index()
        {
            var courses = this.courseService.GetAllAsync<BaseCourseViewModel>();
            var model = new BaseCourseListViewModel { Courses = courses.Result };

            return this.View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            var model = new ImportCourseModel();

            this.GetLanguageList(model);
            this.GetCategoryList(model);
            this.GetDifficultyList(model);

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = await this.filterCourse.GetByIdAsync<ImportCourseModel>(id);

            if (model.Owner.Id != userId)
            {
                return this.Unauthorized();
            }

            this.GetLanguageList(model);
            this.GetCategoryList(model);
            this.GetDifficultyList(model);

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(ImportCourseModel course)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.courseService.UpdateAsync(course, userId);

            return this.Redirect("/");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await this.filterCourse.GetByIdAsync<CourseViewModel>(id);

            return this.View(model);
        }

        // TODO: These private methods should be moved into the services.
        private void GetDifficultyList(ImportCourseModel model)
        {
            model.Difficulties = this.difficultyService.GetAll()
                .Select(keyValuePair => new SelectListItem()
                {
                    Text = keyValuePair.Value.ToString(),
                    Value = keyValuePair.Key.ToString(),
                });
        }

        private void GetCategoryList(ImportCourseModel model)
        {
            model.Categories = this.categoryService.GetAll<CategoryViewModel>()
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
        }

        private void GetLanguageList(ImportCourseModel model)
        {
            model.Languages = this.languageService.GetAllLanguage<LanguageViewModel>().OrderBy(x => x.Name)
                            .Select(x => new SelectListItem()
                            {
                                Text = x.Name,
                                Value = x.Id.ToString(),
                            }).ToList();
        }
    }
}
