// ReSharper disable VirtualMemberCallInConstructor
namespace LearnFast.Data.Models
{
    using System;
    using System.Runtime;
    using LearnFast.Data.Common.Models;

    public class Image : IDeletableEntity
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string UrlPath { get; set; }

        public Course Course { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
