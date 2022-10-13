namespace LearnFast.Web.ViewModels.Course
{
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.CourseContent;
    using LearnFast.Web.ViewModels.Review;

    public class CourseViewModel : BaseCourseViewModel, IMapFrom<Course>
    {
        public string Description { get; set; }

        public string Requirments { get; set; }

        public CourseContentView Content { get; set; }

        public ReviewListViewModel Reviews { get; set; }

    }
}
