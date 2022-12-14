namespace LearnFast.Web.ViewModels.Course
{
    using System.Collections.Generic;

    public class BaseCourseListViewModel
    {
        public IEnumerable<BaseCourseViewModel> Courses { get; set; }
    }
}
