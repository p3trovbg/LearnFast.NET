namespace LearnFast.Web.ViewModels.CourseContent
{
    using System.Collections.Generic;

    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;

    public class CourseContentView : IMapFrom<CourseContent>
    {
        public IEnumerable<Image> Images { get; set; }

        public IEnumerable<Video> Videos { get; set; }
    }
}
