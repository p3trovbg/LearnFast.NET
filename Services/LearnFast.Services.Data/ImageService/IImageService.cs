namespace LearnFast.Services.Data.ImageService
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using LearnFast.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IImageService
    {
        public Task<Image> UploadImage(IFormFile imageFile, string nameFolder);
    }
}
