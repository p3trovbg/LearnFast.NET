namespace LearnFast.Data.Models
{
    using System.Collections.Generic;

    using LearnFast.Data.Common.Models;

    public class CourseContent : BaseDeletableModel<int>
    {
        public CourseContent()
        {
            this.Images = new HashSet<Image>();
            this.Videos = new HashSet<Video>();
        }

        public string Name { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Video> Videos { get; set; }
    }
}
