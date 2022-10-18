namespace LearnFast.Services.Data.VideoService
{
    using System.Threading.Tasks;

    using LearnFast.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IVideoService
    {
        public Task<Video> UploadVideo(IFormFile imageFile, string videoTitle, string nameFolder);
    }
}
