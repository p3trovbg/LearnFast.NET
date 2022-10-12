namespace LearnFast.Data.Models
{
    using System;

    using LearnFast.Data.Common.Models;

    public class StudentCourse : BaseDeletableModel<int>
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
    }
}
