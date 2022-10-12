namespace LearnFast.Web.ViewModels.ApplicationUser
{
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;

    public class BaseUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
    }
}
