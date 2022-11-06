namespace LearnFast.Web.ViewModels.Review
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Services.Mapping.PropertyCopier;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditReviewViewModel : IMapFrom<Review>
    {
        [NotCopy]
        public string UserId { get; set; }

        [NotCopy]
        public int CourseId { get; set; }

        [NotCopy]
        public int ReviewId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int Rating { get; set; }

        [NotCopy]
        public IEnumerable<SelectListItem> RatingList { get; set; }
    }
}
