namespace LearnFast.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Course;

    public class HomeViewModel
    {
        public IEnumerable<BaseCourseViewModel> Courses { get; set; }
    }
}
