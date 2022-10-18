namespace LearnFast.Services.Data.VideoService
{
    using System;
    using System.Threading.Tasks;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using Microsoft.AspNetCore.Http;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Video = LearnFast.Data.Models.Video;

    public class VideoService : IVideoService
    {
        private const string Folder = "videos";

        private readonly Cloudinary cloudinary;
        private readonly IDeletableEntityRepository<Video> videoRepository;

        public VideoService(IDeletableEntityRepository<Video> videoRepository, Cloudinary cloudinary)
        {
            this.videoRepository = videoRepository;
            this.cloudinary = cloudinary;
        }

        public async Task<Video> UploadVideo(IFormFile videoFile, string videoTitle, string nameFolder = Folder)
        {
            using var stream = videoFile.OpenReadStream();
            var video = new Video();
            video.Title = videoTitle;
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(video.Id, stream),
                Overwrite = true,
                Folder = nameFolder,
            };

            var result = await this.cloudinary.UploadAsync(uploadParams);
            video.UrlPath = result.Url.ToString();

            await this.videoRepository.AddAsync(video);
            await this.videoRepository.SaveChangesAsync();

            return video;
        }
    }
}
