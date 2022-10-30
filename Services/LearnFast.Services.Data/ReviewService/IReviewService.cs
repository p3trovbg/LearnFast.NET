using LearnFast.Web.ViewModels.Review;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnFast.Services.Data.ReviewService
{
    public interface IReviewService
    {
        Task<IEnumerable<T>> GetSelectedReviewsByCourse<T>(int courseId);

        Task<IEnumerable<T>> GetAllReviewsByCourse<T>(int courseId);

        Task Add(ImportReviewViewModel model);
    }
}
