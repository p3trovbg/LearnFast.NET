namespace LearnFast.Web.ViewModels.Review
{
    using System.Collections.Generic;
    using System.Linq;

    public class ReviewListViewModel
    {
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}
