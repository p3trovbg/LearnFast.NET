namespace LearnFast.Services
{
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using LearnFast.Common;
    using LearnFast.Data.Models;
    using Microsoft.AspNetCore.Http;

    using Video = LearnFast.Data.Models.Video;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile image, string imageId)
        {
            using var stream = image.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageId, stream),
                Folder = GlobalConstants.ImagesFolderName,
            };

            return await this.cloudinary.UploadAsync(uploadParams);
        }

        public async Task<VideoUploadResult> UploadVideoAsync(IFormFile video, string videoId)
        {
            using var stream = video.OpenReadStream();

            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(videoId, stream),
                Folder = GlobalConstants.VideoFolderName,
            };

            return await this.cloudinary.UploadAsync(uploadParams);
        }
    }
}
