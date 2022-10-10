namespace LearnFast.Data.Models
{
    using LearnFast.Data.Common.Models;
    using System;

    public class StudentCourse : IDeletableEntity, IAuditInfo
    {
        public int UserId { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
