namespace LearnFast.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Braintree;
    using Ganss.Xss;
    using LearnFast.Data.Models;
    using LearnFast.Services;
    using LearnFast.Services.Data;
    using LearnFast.Services.Data.CategoryService;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Services.Data.DifficultyService;
    using LearnFast.Services.Data.LanguageService;
    using LearnFast.Web.ViewModels.ApplicationUser;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Filter;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
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
        private readonly IBraintreeService braintreeService;
        private readonly IUserService userService;

        public CourseController(
            ICourseService courseService,
            IFilterCourse filterCourse,
            ILanguageService languageService,
            ICategoryService categoryService,
            IDifficultyService difficultyService,
            IBraintreeService braintreeService,
            IUserService userService)
        {
            this.courseService = courseService;
            this.filterCourse = filterCourse;
            this.languageService = languageService;
            this.categoryService = categoryService;
            this.difficultyService = difficultyService;
            this.braintreeService = braintreeService;
            this.userService = userService;
        }

        public static string CourseNameController => nameof(CourseController).Replace("Controller", string.Empty);

        public static string DetailsActionName => nameof(Details);

        public IActionResult IsPaid()
        {
            return this.View();
        }

        public async Task<IActionResult> Create(bool isPaid = false)
        {
            var user = await this.userService.GetLoggedUserAsync();

            if (user.StripeId == null && isPaid == true)
            {
                return this.RedirectToAction(nameof(this.IsPaid), CourseNameController);
            }

            var model = new ImportCourseModel();
            model.IsFree = isPaid;
            await this.LoadingBaseParameters(model);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ImportCourseModel model)
        {
            if (!this.ModelState.IsValid)
            {
                await this.LoadingBaseParameters(model);

                return this.View(model);
            }

            try
            {
                var course = await this.courseService.AddCourseAsync(model);
                var courseId = course.Id;

                return this.RedirectToAction(nameof(this.Details), new { id = courseId });
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = await this.userService.GetLoggedUserIdAsync();
            try
            {
                await this.courseService.DeleteCourseByIdAsync(id, userId);
                return this.RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = await this.userService.GetLoggedUserIdAsync();

            try
            {
                var model = await this.filterCourse.GetCourseByIdAsync<ImportCourseModel>(id);

                if (model.Owner.Id != userId)
                {
                    return this.Forbid();
                }

                await this.LoadingBaseParameters(model);

                return this.View(model);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ImportCourseModel model)
        {
            if (!this.ModelState.IsValid)
            {
                await this.LoadingBaseParameters(model);

                return this.View(model);
            }

            try
            {
                var userId = await this.userService.GetLoggedUserIdAsync();
                await this.courseService.UpdateAsync(model, userId);
                return this.RedirectToAction(nameof(this.Details), new { id = model.Id });
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        public async Task<IActionResult> EnrollFree(int courseId)
        {
            try
            {
                var userId = await this.userService.GetLoggedUserIdAsync();
                await this.courseService.EnrollCourse(courseId, userId);

                return this.RedirectToAction(nameof(this.Details), new { id = courseId });
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Buy(int id)
        {
            try
            {
                var course = await this.filterCourse.GetCourseByIdAsync<PurchaseCourseViewModel>(id);

                var gateway = this.braintreeService.GetGateway();
                var clientToken = await gateway.ClientToken.GenerateAsync();
                this.ViewData["ClientToken"] = clientToken;

                return this.View(course);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Buy(PurchaseCourseViewModel model)
        {
            var gateway = this.braintreeService.GetGateway();
            var request = new TransactionRequest
            {
                Amount = model.Price,
                PaymentMethodNonce = model.Nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                },
            };

            var customer = await gateway.Customer.CreateAsync();
            Result<Transaction> result = await gateway.Transaction.SaleAsync(request);

            if (result.IsSuccess())
            {
                try
                {
                    var userId = await this.userService.GetLoggedUserIdAsync();
                    await this.courseService.EnrollCourse(model.Id, userId);

                    return this.RedirectToAction(nameof(this.Details), new { id = model.Id });
                }
                catch (Exception ex)
                {
                    return this.BadRequest(ex.Message);
                }
            }

            return this.View(model.Id);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await this.filterCourse.GetCourseByIdAsync<CourseViewModel>(id);
                var user = await this.userService.GetLoggedUserAsync();

                model.IsUserEnrolled = this.filterCourse.IsUserEnrolledCourse(user.Id, id);
                model.CurrentUserId = user.Id;

                var sanitizer = new HtmlSanitizer();
                model.Description = sanitizer.Sanitize(model.Description);
                model.Requirments = sanitizer.Sanitize(model.Requirments);

                return this.View(model);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Search(SearchViewModel model)
        {
            try
            {
                await this.courseService.SearchCourses(model);
                await LoadingBaseParameters(model);
                return this.View(CoursesView, model);
            }
            catch (Exception)
            {
                return this.RedirectToAction(EmptyView, CategoryController);
            }
        }

        private async Task LoadingBaseParameters(ImportCourseModel model)
        {
            model.Languages = await this.languageService.GetLanguagesAsSelectListItem();
            model.Categories = await this.categoryService.GetCategoryList();
            model.Difficulties = this.difficultyService.GetDifficultyList();
        }

        private async Task LoadingBaseParameters(SearchViewModel model)
        {
            model.Languages = await this.languageService.GetLanguagesAsSelectListItem();
            model.Categories = await this.categoryService.GetCategoryList();
            model.Difficulties = this.difficultyService.GetDifficultyList();
        }
    }
}
