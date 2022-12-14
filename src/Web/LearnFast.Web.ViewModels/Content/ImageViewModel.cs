using LearnFast.Data.Models;
using LearnFast.Services.Mapping;

namespace LearnFast.Web.ViewModels.Content
{
    public class ImageViewModel : IMapFrom<Image>
    {
        public string ImageUrl { get; set; }
    }
}
