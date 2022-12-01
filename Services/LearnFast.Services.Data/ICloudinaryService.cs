namespace LearnFast.Services
{
    using System.Threading.Tasks;

    using CloudinaryDotNet.Actions;
    using LearnFast.Data.Models;
    using Microsoft.AspNetCore.Http;

    using Video = LearnFast.Data.Models.Video;

    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile image, Image entity);

        Task<VideoUploadResult> UploadVideoAsync(IFormFile video, Video entity);
    }
}
