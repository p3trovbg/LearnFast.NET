namespace LearnFast.Web.ViewModels.Content
{
    using LearnFast.Services.Mapping.PropertyCopier;
    using Microsoft.AspNetCore.Http;

    public class EditVideoViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        [NotCopy]
        public string UrlPath { get; set; }

        [NotCopy]
        public int CourseId { get; set; }

        [NotCopy]
        public IFormFile VideoFile { get; set; }

        public string Description { get; set; }
    }
}
