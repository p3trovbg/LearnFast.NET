namespace LearnFast.Web.ViewModels.Review
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ImportReviewViewModel
    {
        [Required]
        [StringLength(200)]
        public string Content { get; set; }

        public int CourseId { get; set; }

        public string UserId { get; set; }

        [Required]
        public int Rating { get; set; }

        public IEnumerable<SelectListItem> RatingList { get; set; }
    }
}
