namespace LearnFast.Web.ViewModels.Course
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.ApplicationUser;
    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Language;

    public class BaseCourseViewModel : IMapFrom<Course>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string MainImageUrl { get; set; }

        [Display(Name = "Free:")]
        public bool IsFree { get; set; }

        public string Difficulty { get; set; }

        public int Sells { get; set; }

        public LanguageViewModel Language { get; set; }

        public CategoryViewModel Category { get; set; }

        public BaseUserViewModel Owner { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Course, CourseViewModel>()
                 .ForMember(
                d => d.Difficulty,
                m => m.MapFrom(x => x.Difficulty.ToString()));
        }
    }
}
