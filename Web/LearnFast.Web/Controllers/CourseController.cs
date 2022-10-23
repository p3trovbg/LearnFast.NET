namespace LearnFast.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using LearnFast.Data.Models;
    using LearnFast.Data.Models.Enums;
    using LearnFast.Services.Data;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Filter;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class CourseController : BaseController
    {
        private const string OrderByName = "name";
        private const string OrderByPrice = "price";
        private const string OrderDescByPrice = "desc_price";

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

            model.Languages = await this.languageService.GetLanguageListAsync();
            model.Categories = await this.categoryService.GetCategoryList();
            model.Difficulties = this.difficultyService.GetDifficultyList();

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

            model.Languages = await this.languageService.GetLanguageListAsync();
            model.Categories = await this.categoryService.GetCategoryList();
            model.Difficulties = this.difficultyService.GetDifficultyList();

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

            return this.Redirect($"/course/details?id={course.Id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await this.filterCourse.GetByIdAsync<CourseViewModel>(id);

            return this.View(model);
        }

        public async Task<IActionResult> Search(FilterViewModel model)
        {
            var coursesAsQuery = this.courseService.GetAllAsQueryAble<BaseCourseViewModel>();

            if (!string.IsNullOrEmpty(model.SearchString))
            {
                coursesAsQuery = coursesAsQuery.Where(x => x.Title.ToLower().Contains(model.SearchString.ToLower()));
            }

            if (model.CategoryId != null)
            {
                coursesAsQuery = coursesAsQuery.Where(x => x.Category.Id == model.CategoryId);

                if (!coursesAsQuery.Any())
                {
                    return this.RedirectToAction("Empty", "Category");
                }
                else
                {
                    model.CategoryName = this.categoryService.GetCategoryName(model.CategoryId);
                }
            }

            if (model.FinalPrice > 0 && model.IsFree == false)
            {
                coursesAsQuery = coursesAsQuery.Where(x => x.Price >= model.InitialPrice && x.Price <= model.FinalPrice);
            }

            if (model.LanguageId != null)
            {
                coursesAsQuery = coursesAsQuery.Where(x => x.Language.Id == model.LanguageId);
            }

            if (model.Difficulty != null)
            {
                var diff = (Difficulty)model.Difficulty;
                coursesAsQuery = (IQueryable<BaseCourseViewModel>)coursesAsQuery.AsEnumerable().Where(x => x.Difficulty == diff.ToString());
            }

            if (model.IsFree)
            {
                coursesAsQuery = coursesAsQuery.Where(x => x.IsFree);
            }

            model.Courses = await coursesAsQuery.ToListAsync();
            await this.GetDefaultModelProps(model);

            return this.View("Courses", model);
        }

        private async Task GetDefaultModelProps(FilterViewModel model)
        {
            model.Difficulties = this.difficultyService.GetDifficultyList();
            model.Categories = await this.categoryService.GetCategoryList();
            model.Languages = await this.languageService.GetLanguageListAsync();
            model.Sorter = new List<SelectListItem>
            {
                new SelectListItem { Text = "Order by price", Value = OrderByPrice },
                new SelectListItem { Text = "Order by desc price", Value = OrderDescByPrice },
                new SelectListItem { Text = "Order by name", Value = OrderByName },
            };
        }
    }
}
