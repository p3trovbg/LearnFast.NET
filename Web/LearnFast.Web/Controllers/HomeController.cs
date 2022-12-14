namespace LearnFast.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CategoryService;
    using LearnFast.Services.Data.ContactService;
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
        private readonly IContactService contactService;

        public HomeController(
            IFilterCourse filterCourse,
            ICategoryService categoryService,
            UserManager<ApplicationUser> userManager,
            IContactService contactService)
        {
            this.filterCourse = filterCourse;
            this.categoryService = categoryService;
            this.userManager = userManager;
            this.contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel();
            model.Courses = await this.filterCourse.GetTop12BestSellersCourses<BaseCourseViewModel>();
            return this.View(model);
        }

        public IActionResult Contact(bool isSuccessfully)
        {
            var model = new InputContactViewModel();
            model.IsSuccessfully = isSuccessfully;
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(InputContactViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.contactService.AcceptingMessage(model);

                return this.RedirectToAction(nameof(this.Contact), new { isSuccessfully = true });
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [Route("/About")]
        public IActionResult About()
        {
            return this.View();
        }

        [Route("/Privacy")]
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

        [Route("/Home/ErrorView/{status:int}")]
        public IActionResult ErrorView(int status)
        {
            return this.View("~/Views/Shared/Error404.cshtml");
        }
    }
}
