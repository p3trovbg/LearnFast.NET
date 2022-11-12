namespace LearnFast.Web.ViewModels.Review
{
    using System.Collections.Generic;
    using System.Linq;

    public class ReviewListViewModel : PagingViewModel
    {
        public int CourseId { get; set; }

        public bool IsEmpty => this.Reviews.Count() == 0;

        public bool IsSelectedReviews { get; set; }

        public string CurrentUserId { get; set; }

        public string CourseOwnerId { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}
