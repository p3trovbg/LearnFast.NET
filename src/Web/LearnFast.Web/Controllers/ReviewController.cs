namespace LearnFast.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using LearnFast.Services.Data;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Services.Data.ReviewService;
    using LearnFast.Web.ViewModels.Review;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize]
    public class ReviewController : BaseController
    {
        private readonly IReviewService reviewService;
        private readonly ICourseService courseService;
        private readonly IUserService userService;

        public ReviewController(
            IReviewService reviewService,
            ICourseService courseService,
            IUserService userService)
        {
            this.reviewService = reviewService;
            this.courseService = courseService;
            this.userService = userService;
        }

        public async Task<IActionResult> All(ReviewListViewModel model)
        {
            try
            {
                await this.reviewService.GetAllReviewsByCourse(model);

                model.CourseOwnerId = await this.courseService.GetOwnerIdByCourse(model.CourseId);
                model.CurrentUserId = await this.userService.GetLoggedUserIdAsync();

                return this.View(model);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        public IActionResult Add(int courseId)
        {
            var model = new ImportReviewViewModel();
            model.CourseId = courseId;
            model.RatingList = LoadRatings();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ImportReviewViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                model.UserId = await this.userService.GetLoggedUserIdAsync();
                await this.reviewService.Add(model);
            }
            catch (Exception ex)
            {
                this.NotFound(ex.Message);
            }

            return this.RedirectToAction(nameof(this.All), new { CourseId = model.CourseId });
        }

        public async Task<IActionResult> Delete(int reviewId, int courseId)
        {
            try
            {
                var userId = await this.userService.GetLoggedUserIdAsync();
                await this.reviewService.Delete(reviewId, userId);
            }
            catch (Exception ex)
            {
                this.NotFound(ex.Message);
            }

            return this.RedirectToAction(nameof(this.All), new { CourseId = courseId });
        }

        public async Task<IActionResult> Edit(int reviewId)
        {
            var currentUserId = await this.userService.GetLoggedUserIdAsync();

            try
            {
                var model = await this.reviewService.GetReviewById<EditReviewViewModel>(reviewId);
                model.RatingList = LoadRatings();
                if (currentUserId != model.UserId)
                {
                    return this.Forbid();
                }

                return this.View(model);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditReviewViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var currentUserId = await this.userService.GetLoggedUserIdAsync();

            if (currentUserId != model.UserId)
            {
                return this.Forbid();
            }

            await this.reviewService.Edit(model);

            return this.RedirectToAction(nameof(this.All), new { CourseId = model.CourseId });
        }

        public async Task<IActionResult> Selecting(SelectingReviewViewModel model)
        {
            try
            {
                await this.reviewService.Selecting(model);

                return this.RedirectToAction(CourseController.DetailsActionName, CourseController.CourseNameController, new { id = model.CourseId });
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        private static List<SelectListItem> LoadRatings()
        {
            var ratings = new List<SelectListItem>();
            for (int rating = 1; rating <= 6; rating++)
            {
                ratings.Add(new SelectListItem
                {
                    Text = rating.ToString(),
                    Value = rating.ToString(),
                });
            }

            return ratings;
        }

    }
}
