// ReSharper disable VirtualMemberCallInConstructor
namespace LearnFast.Data.Models
{
    using System;

    using LearnFast.Data.Common.Models;

    public class Video : IDeletableEntity
    {
        public Video()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public Course Course { get; set; }

        public string UrlPath { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
