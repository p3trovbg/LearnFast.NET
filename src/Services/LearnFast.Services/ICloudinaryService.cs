namespace LearnFast.Services
{
    using System.Threading.Tasks;

    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile image, string imageId);

        Task<VideoUploadResult> UploadVideoAsync(IFormFile video, string videoid);
    }
}
