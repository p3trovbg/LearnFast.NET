namespace LearnFast.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using LearnFast.Services.Data.ReviewService;
    using LearnFast.Web.ViewModels.Review;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize]
    public class ReviewController : BaseController
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        public async Task<IActionResult> All(ReviewListViewModel model)
        {
            try
            {
                await this.reviewService.GetAllReviewsByCourse(model);
                model.CurrentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

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
            var ratings = new List<SelectListItem>();
            for (int rating = 1; rating <= 6; rating++)
            {
                ratings.Add(new SelectListItem
                {
                    Text = rating.ToString(),
                    Value = rating.ToString(),
                });
            }

            model.RatingList = ratings;

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
                model.UserId = model.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                await this.reviewService.Add(model);
            }
            catch (Exception ex)
            {
                this.NotFound(ex.Message);
            }

            return this.RedirectToAction("Details", "Course", new { id = model.CourseId });
        }

        public async Task<IActionResult> Delete(int reviewId, string userId, int courseId)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != userId)
            {
                this.NotFound();
            }

            try
            {
                await this.reviewService.Delete(reviewId);
            }
            catch (Exception ex)
            {
                this.NotFound(ex.Message);
            }

            return this.RedirectToAction(nameof(this.All), new { CourseId = courseId });
        }
    }
}
