namespace LearnFast.Services.Data.ReviewService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Review;

    public interface IReviewService
    {
        Task<IEnumerable<T>> GetSelectedReviewsByCourse<T>(int courseId);

        Task GetAllReviewsByCourse(ReviewListViewModel model);

        Task<T> GetReviewById<T>(int reviewId);

        Task Add(ImportReviewViewModel model);

        Task Delete(int reviewId, string userId);

        Task Edit(EditReviewViewModel model);

        Task Selecting(SelectingReviewViewModel model);

        Task<int> GetReviewsCountAsync();
    }
}
