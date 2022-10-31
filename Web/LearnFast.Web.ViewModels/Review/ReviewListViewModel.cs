namespace LearnFast.Web.ViewModels.Review
{
    using System.Collections.Generic;

    public class ReviewListViewModel : PagingViewModel
    {
        public int CourseId { get; set; }

        public bool IsSelectedReviews { get; set; }

        public string CurrentUserId { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}
