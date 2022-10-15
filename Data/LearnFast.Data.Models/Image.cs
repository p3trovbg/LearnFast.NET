// ReSharper disable VirtualMemberCallInConstructor
namespace LearnFast.Data.Models
{
    using System;

    using LearnFast.Data.Common.Models;

    public class Image : IDeletableEntity
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string UrlPath { get; set; }

        public int ContentId { get; set; }

        public CourseContent Content { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
