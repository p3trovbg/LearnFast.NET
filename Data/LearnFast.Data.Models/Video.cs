// ReSharper disable VirtualMemberCallInConstructor
namespace LearnFast.Data.Models
{
    using LearnFast.Data.Common.Models;

    public class Video : BaseDeletableModel<int>
    {
        public string Path { get; set; }

        public int ContentId { get; set; }

        public virtual Content Content { get; set; }
    }
}
