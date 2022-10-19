namespace LearnFast.Web.Controllers
{
    using System.Threading.Tasks;

    using LearnFast.Services.Data;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Mvc;

    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IFilterCourse filterCourse;

        public CategoryController(ICategoryService categoryService, IFilterCourse filterCourse)
        {
            this.categoryService = categoryService;
            this.filterCourse = filterCourse;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAll<CategoryViewModel>();

            var model = new CategoryListViewModel { Categories = categories };

            return this.View(model);
        }

        public async Task<IActionResult> CoursesByCategory(int id)
        {
            var courses = await this.filterCourse.GetCoursesByCategoryAsync<BaseCourseViewModel>(id);

            var model = new CategoryViewModel { Courses = courses };

            return this.View("CategoryCourses", model);
        }
    }
}
