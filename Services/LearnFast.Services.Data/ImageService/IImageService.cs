namespace LearnFast.Services.Data.ImageService
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;

    public interface IImageService
    {
        public Task UploadImage(IFormFile imageFile, string nameFolder);
    }
}
