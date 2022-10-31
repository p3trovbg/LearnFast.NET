namespace LearnFast.Web.ViewModels.Review
{
    using System.Collections.Generic;

    public class ReviewListViewModel : PagingViewModel
    {
        public int CourseId { get; set; }

        public bool IsSelected { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}
