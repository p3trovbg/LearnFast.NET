namespace LearnFast.Web.ViewModels.Course
{
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;

    public class CourseProfileViewModel : IMapFrom<Course>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
