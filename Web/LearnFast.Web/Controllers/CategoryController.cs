namespace LearnFast.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using LearnFast.Services.Data;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Filter;
    using Microsoft.AspNetCore.Mvc;

    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IFilterCourse filterCourse;
        private readonly ILanguageService languageService;
        private readonly IDifficultyService difficultyService;

        public CategoryController(
            ICategoryService categoryService,
            IFilterCourse filterCourse,
            ILanguageService languageService,
            IDifficultyService difficultyService)
        {
            this.categoryService = categoryService;
            this.filterCourse = filterCourse;
            this.languageService = languageService;
            this.difficultyService = difficultyService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAllAsync<CategoryViewModel>();

            var model = new CategoryListViewModel { Categories = categories };

            return this.View(model);
        }

        public async Task<IActionResult> Courses(int id, string name)
        {
            var courses = await this.filterCourse.GetCoursesByCategoryAsync<BaseCourseViewModel>(id);

            if (!courses.Any())
            {
                return this.RedirectToAction("Empty", "Category");
            }

            var model = new FilterViewModel { Courses = courses };

            model.CategoryName = name;
            model.Difficulties = this.difficultyService.GetDifficultyList();
            model.Categories = await this.categoryService.GetCategoryList();
            model.Languages = await this.languageService.GetLanguageListAsync();

            return this.View(model);
        }

        public IActionResult Empty()
        {
            return this.View();
        }
    }
}
