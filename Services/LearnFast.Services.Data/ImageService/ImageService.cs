namespace LearnFast.Services.Data.ImageService
{
    using System;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using Microsoft.AspNetCore.Http;

    public class ImageService : IImageService
    {
        private readonly IDeletableEntityRepository<Image> imageRepository;
        private readonly Cloudinary cloudinary;

        public ImageService(
            IDeletableEntityRepository<Image> imageRepository,
            Cloudinary cloudinary)
        {
            this.imageRepository = imageRepository;
            this.cloudinary = cloudinary;
        }

        public async Task<Image> UploadImage(IFormFile imageFile, string nameFolder)
        {
            using var stream = imageFile.OpenReadStream();
            var image = new Image();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(image.Id, stream),
                Folder = nameFolder,
            };

            var result = await this.cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
            {
                throw new InvalidOperationException(result.Error.Message);
            }

            image.UrlPath = result.Url.ToString();

            await this.imageRepository.AddAsync(image);
            await this.imageRepository.SaveChangesAsync();

            return image;
        }
    }
}
