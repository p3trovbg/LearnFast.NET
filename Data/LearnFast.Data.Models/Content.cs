// ReSharper disable VirtualMemberCallInConstructor
namespace LearnFast.Data.Models
{
    using System.Collections.Generic;

    using LearnFast.Data.Common.Models;

    public class Content : BaseDeletableModel<int>
    {
        public Content()
        {
            this.Images = new HashSet<Image>();
            this.Videos = new HashSet<Video>();
        }

        public virtual Course Course { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Video> Videos { get; set; }
    }
}
