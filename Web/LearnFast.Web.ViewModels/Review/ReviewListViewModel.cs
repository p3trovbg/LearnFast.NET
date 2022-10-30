namespace LearnFast.Web.ViewModels.Review
{
    using System.Collections.Generic;

    public class ReviewListViewModel : PagingViewModel
    {
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}
