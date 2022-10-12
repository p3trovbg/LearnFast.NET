namespace LearnFast.Web
{
    using AutoMapper;
    using LearnFast.Data.Models;
    using LearnFast.Web.ViewModels.Course;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<ImportCourseModel, Course>();
        }
    }
}
