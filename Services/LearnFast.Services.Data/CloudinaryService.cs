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

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile image, Image entity)
        {
            using var stream = image.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(entity.Id, stream),
                Folder = GlobalConstants.ImagesFolderName,
            };

            return await this.cloudinary.UploadAsync(uploadParams);
        }

        public async Task<VideoUploadResult> UploadVideoAsync(IFormFile video, Video entity)
        {
            using var stream = video.OpenReadStream();

            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(entity.Id, stream),
                Folder = GlobalConstants.VideoFolderName,
            };

            return await this.cloudinary.UploadAsync(uploadParams);
        }
    }
}
