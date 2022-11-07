namespace LearnFast.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Course;

    public class HomeViewModel
    {
        public IEnumerable<BaseCourseViewModel> Courses { get; set; }

        public IEnumerable<BaseCourseViewModel> Users { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
