namespace LearnFast.Web.ViewModels.Content
{
    using System.ComponentModel.DataAnnotations;

    using LearnFast.Common;
    using Microsoft.AspNetCore.Http;

    public class ImportVideoModel
    {
        [Required]
        [StringLength(GlobalConstants.MaxVideoTitle, MinimumLength = GlobalConstants.MinVideoTitle)]
        public string Title { get; set; }

        [Required]
        public int CourseId { get; set; }

        [StringLength(GlobalConstants.MaxVideoDescription)]
        public string Description { get; set; }

        [Required]
        public IFormFile VideoFile { get; set; }
    }
}
