namespace LearnFast.Web.ViewModels.Review
{
    using LearnFast.Web.ViewModels.ApplicationUser;
    using LearnFast.Web.ViewModels.Course;

    public class ReviewViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        public CourseViewModel Course { get; set; }

        public BaseUserViewModel User { get; set; }
    }
}
