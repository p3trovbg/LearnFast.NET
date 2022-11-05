namespace LearnFast.Web
{
    using AutoMapper;
    using LearnFast.Data.Models;
    using LearnFast.Web.ViewModels.ApplicationUser;
    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Content;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Language;
    using LearnFast.Web.ViewModels.Review;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<ImportCourseModel, Course>();
            this.CreateMap<ApplicationUser, UserViewModel>();
            this.CreateMap<Course, CourseViewModel>();
            this.CreateMap<Course, BaseCourseViewModel>();
            this.CreateMap<LanguageViewModel, Language>();
            this.CreateMap<CategoryViewModel, Category>();
            this.CreateMap<Video, ImportVideoModel>();
            this.CreateMap<ImportReviewViewModel, Review>();
        }
    }
}
