namespace LearnFast.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using LearnFast.Common;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CategoryService;
    using LearnFast.Services.Data.CountryService;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Services.Data.LanguageService;
    using LearnFast.Services.Data.ReviewService;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.Administration.Dashboard;
    using LearnFast.Web.ViewModels.ApplicationUser;
    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Country;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Language;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Build.Framework;
    using Microsoft.EntityFrameworkCore;

    public class DashboardController : AdministrationController
    {
        private UserManager<ApplicationUser> userManager;
        private ICategoryService categoryService;
        private ILanguageService languageService;
        private ICountryService countryService;
        private ICourseService courseService;
        private IReviewService reviewService;

        public DashboardController(
            UserManager<ApplicationUser> userManager,
            ICategoryService categoryService,
            ILanguageService languageService,
            ICountryService countryService,
            ICourseService courseService,
            IReviewService reviewService)
        {
            this.userManager = userManager;
            this.categoryService = categoryService;
            this.languageService = languageService;
            this.countryService = countryService;
            this.courseService = courseService;
            this.reviewService = reviewService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel();

            var allUsers = await this.userManager.Users.To<BaseUserViewModel>().ToListAsync();
            var allCategories = await this.categoryService.GetAllAsync<CategoryViewModel>();
            var allLanguages = await this.languageService.GetAllLanguageAsync<LanguageViewModel>();
            var allCourses = await this.courseService.GetAllAsync<BaseCourseViewModel>();
            var allCountries = await this.countryService.GetAllOrderByAlphabeticalAsync<CountryViewModel>();
            var coursesCount = await this.courseService.GetCountAsync();
            var reviewsCount = await this.reviewService.GetReviewsCountAsync();

            model.Users = allUsers;
            model.Categories = allCategories;
            model.Languages = allLanguages;
            model.Countries = allCountries;
            model.CoursesCount = coursesCount;
            model.ReviewsCount = reviewsCount;
            model.Courses = allCourses;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var user = await this.userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    throw new ArgumentException(GlobalExceptions.UserNotExists);
                }

                await this.userManager.DeleteAsync(user);

                return this.RedirectToAction(nameof(this.Index));
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }
    }
}
