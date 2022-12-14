namespace LearnFast.Web.ViewModels.Review
{
    using LearnFast.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using SelectListItem = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

    public class ImportReviewViewModel
    {
        [Required]
        [StringLength(GlobalConstants.MaxReviewContentLength, MinimumLength = GlobalConstants.MinReviewContentLength)]
        public string Content { get; set; }

        public int CourseId { get; set; }

        public string UserId { get; set; }

        [Required]
        [Range(GlobalConstants.MinReviewRange, GlobalConstants.MaxReviewRange)]
        public int Rating { get; set; }

        public IEnumerable<SelectListItem> RatingList { get; set; }
    }
}
