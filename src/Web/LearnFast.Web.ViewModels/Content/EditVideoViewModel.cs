namespace LearnFast.Web.ViewModels.Content
{
    using System.ComponentModel.DataAnnotations;

    using LearnFast.Common;
    using LearnFast.Services.Mapping.PropertyCopier;
    using Microsoft.AspNetCore.Http;

    public class EditVideoViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.MaxVideoTitle, MinimumLength = GlobalConstants.MinVideoTitle)]
        public string Title { get; set; }

        [NotCopy]
        [Required]
        public string UrlPath { get; set; }

        [NotCopy]
        [Required]
        public int CourseId { get; set; }

        [NotCopy]
        public IFormFile VideoFile { get; set; }

        [StringLength(GlobalConstants.MaxVideoDescription)]
        public string Description { get; set; }
    }
}
