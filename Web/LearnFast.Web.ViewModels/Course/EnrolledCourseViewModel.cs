namespace LearnFast.Web.ViewModels.Course
{
    using AutoMapper;
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;

    public class EnrolledCourseViewModel : IMapFrom<StudentCourse>, IHaveCustomMappings
    {
        public int CourseId { get; set; }

        public string Title { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<StudentCourse, EnrolledCourseViewModel>()
                 .ForMember(
                d => d.Title,
                m => m.MapFrom(x => x.Course.Title));
        }
    }
}
