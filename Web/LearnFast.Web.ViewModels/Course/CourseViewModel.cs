namespace LearnFast.Web.ViewModels.Course
{
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;

    public class CourseViewModel : BaseCourseViewModel, IMapFrom<Course>
    {
        public string Description { get; set; }

        public string Requirments { get; set; }

        // TODO: Add videos and images model view
    }
}
