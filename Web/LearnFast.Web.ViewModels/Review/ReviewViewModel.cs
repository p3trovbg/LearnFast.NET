namespace LearnFast.Web.ViewModels.Review
{
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.ApplicationUser;

    public class ReviewViewModel : IMapFrom<Review>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        public BaseUserViewModel User { get; set; }
    }
}
