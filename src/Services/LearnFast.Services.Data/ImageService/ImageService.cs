namespace LearnFast.Services.Data.ImageService
{
    using System;
    using System.Threading.Tasks;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using Microsoft.AspNetCore.Http;

    public class ImageService : IImageService
    {
        private readonly IDeletableEntityRepository<Image> imageRepository;
        private readonly ICloudinaryService cloudinaryService;

        public ImageService(
            IDeletableEntityRepository<Image> imageRepository,
            ICloudinaryService cloudinaryService)
        {
            this.imageRepository = imageRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<Image> UploadImage(IFormFile imageFile, string nameFolder)
        {
            var image = new Image();

            var result = await this.cloudinaryService.UploadImageAsync(imageFile, image.Id);

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
