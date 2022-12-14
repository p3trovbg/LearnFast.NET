// ReSharper disable VirtualMemberCallInConstructor
namespace LearnFast.Data.Models
{
    using System.Collections.Generic;

    using LearnFast.Data.Common.Models;

    public class Country : BaseDeletableModel<int>
    {
        public Country()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
