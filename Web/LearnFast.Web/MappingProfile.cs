namespace LearnFast.Web
{
    using AutoMapper;
    using LearnFast.Data.Models;
    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Language;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<ImportCourseModel, Course>();
            this.CreateMap<Course, CourseViewModel>();
            this.CreateMap<LanguageViewModel, Language>();
            this.CreateMap<CategoryViewModel, Category>();
        }
    }
}
