namespace LearnFast.Web.ViewModels.Course
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

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

        public DateTime CreatedOn { get; set; }

        public LanguageViewModel Language { get; set; }

        public CategoryViewModel Category { get; set; }

        public BaseUserViewModel Owner { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Course, CourseViewModel>()
                 .ForMember(
                d => d.Difficulty,
                m => m.MapFrom(x => x.Difficulty.ToString()))
                 .ForMember(
                d => d.Reviews,
                m => m.MapFrom(x => x.Reviews.Where(x => x.IsSelected)))
                 .ForMember(
                d => d.Videos,
                m => m.MapFrom(x => x.Videos.OrderBy(x => x.CreatedOn)));
        }
    }
}
