namespace LearnFast.Web.ViewModels.Course
{
    using System.Collections.Generic;
    using System.Linq;

    using LearnFast.Web.ViewModels.Content;
    using LearnFast.Web.ViewModels.Review;

    public class CourseViewModel : BaseCourseViewModel
    {
        public string Description { get; set; }

        public string Requirments { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }

        public IEnumerable<VideoViewModel> Videos { get; set; }

        public IEnumerable<ImageViewModel> Images { get; set; }

        public int ReviewsCount => this.Reviews.Count();

        public int VideosCount => this.Videos.Count();

        public int ImagesCount => this.Images.Count();
    }
}
