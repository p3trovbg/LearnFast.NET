namespace LearnFast.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CategoryService;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Services.Data.ImageService;
    using LearnFast.Web.ViewModels;
    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IFilterCourse filterCourse;
        private readonly ICategoryService categoryService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            IFilterCourse filterCourse,
            ICategoryService categoryService,
            UserManager<ApplicationUser> userManager)
        {
            this.filterCourse = filterCourse;
            this.categoryService = categoryService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel();
            model.Courses = await this.filterCourse.GetTop12BestSellersCourses<BaseCourseViewModel>();
            model.Categories = await this.categoryService.GetAllAsync<CategoryViewModel>();
            return this.View(model);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
