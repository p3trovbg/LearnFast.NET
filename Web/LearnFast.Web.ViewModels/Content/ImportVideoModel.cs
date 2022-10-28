namespace LearnFast.Web.ViewModels.Content
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class ImportVideoModel
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        public int CourseId { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [Required]
        public IFormFile VideoFile { get; set; }
    }
}
