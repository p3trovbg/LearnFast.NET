// ReSharper disable VirtualMemberCallInConstructor
namespace LearnFast.Data.Models
{
    using System.Collections.Generic;

    using LearnFast.Data.Common.Models;

    public class Language : BaseDeletableModel<int>
    {
        public Language()
        {
            this.Courses = new HashSet<Course>();
        }

        public string Nationality { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}