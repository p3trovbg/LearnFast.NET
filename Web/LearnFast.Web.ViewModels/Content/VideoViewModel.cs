namespace LearnFast.Web.ViewModels.Content
{
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;

    public class VideoViewModel : IMapFrom<Video>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string UrlPath { get; set; }

        public string Description { get; set; }
    }
}
