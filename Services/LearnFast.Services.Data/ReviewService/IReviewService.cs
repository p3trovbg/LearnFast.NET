namespace LearnFast.Services.Data.ReviewService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Review;

    public interface IReviewService
    {
        Task<IEnumerable<T>> GetSelectedReviewsByCourse<T>(int courseId);

        Task GetAllReviewsByCourse(ReviewListViewModel model);

        Task Add(ImportReviewViewModel model);
    }
}
