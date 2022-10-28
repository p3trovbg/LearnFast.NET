namespace LearnFast.Services.Data.VideoService
{
    using System.Threading.Tasks;

    using LearnFast.Data.Models;
    using LearnFast.Web.ViewModels.Content;
    using Microsoft.AspNetCore.Http;

    public interface IVideoService
    {
        public Task<string> UploadVideo(ImportVideoModel model);

        public Task RemoveVideo(string videoId);

        public Task EditVideo(EditVideoViewModel model);
    }
}
