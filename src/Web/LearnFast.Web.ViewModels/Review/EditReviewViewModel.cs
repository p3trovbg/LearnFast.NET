namespace LearnFast.Web.ViewModels.Review
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using LearnFast.Common;
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Services.Mapping.PropertyCopier;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditReviewViewModel : IMapFrom<Review>
    {
        [NotCopy]
        [Required]
        public int Id { get; set; }

        [NotCopy]
        [Required]
        public string UserId { get; set; }

        [NotCopy]
        [Required]
        public int CourseId { get; set; }

        [Required]
        [StringLength(GlobalConstants.MaxReviewContentLength, MinimumLength = GlobalConstants.MinReviewContentLength)]
        public string Content { get; set; }

        [Required]
        [Range(GlobalConstants.MinReviewRange, GlobalConstants.MaxReviewRange)]
        public int Rating { get; set; }

        [NotCopy]
        public IEnumerable<SelectListItem> RatingList { get; set; }
    }
}
