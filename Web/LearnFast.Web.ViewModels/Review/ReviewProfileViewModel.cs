namespace LearnFast.Web.ViewModels.Review
{
    using System;

    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;

    public class ReviewProfileViewModel : IMapFrom<Review>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int CourseId { get; set; }

        public string CourseOwnerId { get; set; }

        public int Rating { get; set; }

        public string CourseTitle { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
