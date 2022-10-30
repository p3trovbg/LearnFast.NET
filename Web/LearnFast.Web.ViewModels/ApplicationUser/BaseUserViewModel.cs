namespace LearnFast.Web.ViewModels.ApplicationUser
{
    using AutoMapper;
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;

    public class BaseUserViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string MainImageUrl { get; set; }

        public string Email { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, BaseUserViewModel>()
                .ForMember(
                d => d.FullName,
                m => m.MapFrom(x => x.FirstName + " " + x.LastName))
                .ForMember(
                d => d.MainImageUrl,
                m => m.MapFrom(x => x.MainImageUrl));
        }
    }
}
