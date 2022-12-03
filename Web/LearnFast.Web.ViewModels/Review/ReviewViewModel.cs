namespace LearnFast.Web.ViewModels.Review
{
    using AutoMapper;

    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.ApplicationUser;

    public class ReviewViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        public bool IsSelected { get; set; }

        public string CreatedOn { get; set; }

        public BaseUserViewModel User { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewViewModel>()
                .ForMember(
                d => d.CreatedOn,
                m => m.MapFrom(x => x.CreatedOn.Date.ToString("dd/MM/yyyy")));
        }
    }
}
