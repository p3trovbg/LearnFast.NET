// ReSharper disable VirtualMemberCallInConstructor
namespace LearnFast.Data.Models
{
    using System;

    using LearnFast.Data.Common.Models;

    public class Video : BaseDeletableModel<string>
    {
        public Video()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Title { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public string UrlPath { get; set; }
    }
}
