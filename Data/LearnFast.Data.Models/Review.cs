// ReSharper disable VirtualMemberCallInConstructor
namespace LearnFast.Data.Models
{
    using LearnFast.Data.Common.Models;
    public class Review : BaseDeletableModel<int>
    {
        public int Title { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
