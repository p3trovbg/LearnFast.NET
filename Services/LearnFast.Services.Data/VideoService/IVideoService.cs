namespace LearnFast.Services.Data.VideoService
{
    using System.Threading.Tasks;

    using LearnFast.Data.Models;
    using LearnFast.Web.ViewModels.Content;
    using Microsoft.AspNetCore.Http;

    public interface IVideoService
    {
        public Task UploadVideo(ImportVideoModel model);

        public Task RemoveVideo(string videoId);
    }
}
