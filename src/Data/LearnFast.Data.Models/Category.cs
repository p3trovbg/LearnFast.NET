// ReSharper disable VirtualMemberCallInConstructor
namespace LearnFast.Data.Models
{
    using System.Collections.Generic;

    using LearnFast.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.Courses = new HashSet<Course>();
        }

        public string Name { get; set; }

        public Image? Image { get; set; }

        #nullable enable
        public string? Description { get; set; }
        #nullable disable

        public virtual ICollection<Course> Courses { get; set; }
    }
}
