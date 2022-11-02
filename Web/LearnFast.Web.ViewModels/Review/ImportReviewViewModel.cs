namespace LearnFast.Web.ViewModels.Review
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using SelectListItem = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

    public class ImportReviewViewModel
    {
        [Required]
        public string Content { get; set; }

        public int CourseId { get; set; }

        public string UserId { get; set; }

        [Required]
        public int Rating { get; set; }

        public IEnumerable<SelectListItem> RatingList { get; set; }
    }
}
