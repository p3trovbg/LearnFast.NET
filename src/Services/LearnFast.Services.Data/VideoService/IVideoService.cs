namespace LearnFast.Services.Data.VideoService
{
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Content;

    public interface IVideoService
    {
        public Task<string> UploadVideo(ImportVideoModel model);

        public Task RemoveVideo(string videoId);

        public Task EditVideo(EditVideoViewModel model);
    }
}
