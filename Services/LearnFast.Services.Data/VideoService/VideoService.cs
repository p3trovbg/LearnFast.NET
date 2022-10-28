namespace LearnFast.Services.Data.VideoService
{
    using System;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Web.ViewModels.Content;
    using Microsoft.EntityFrameworkCore;

    using Video = LearnFast.Data.Models.Video;

    public class VideoService : IVideoService
    {
        private const string Folder = "videos";

        private readonly Cloudinary cloudinary;
        private readonly IDeletableEntityRepository<Video> videoRepository;

        public VideoService(
            IDeletableEntityRepository<Video> videoRepository,
            Cloudinary cloudinary)
        {
            this.videoRepository = videoRepository;
            this.cloudinary = cloudinary;
        }

        public async Task RemoveVideo(string videoId)
        {
            var targetVideo = await this.videoRepository.All().FirstOrDefaultAsync(x => x.Id == videoId);

            if (targetVideo == null)
            {
                throw new ArgumentException(GlobalExceptions.VideoIsNotExistExceptionMessage);
            }

            this.videoRepository.Delete(targetVideo);
            await this.videoRepository.SaveChangesAsync();
        }

        public async Task UploadVideo(ImportVideoModel model)
        {
            using var stream = model.VideoFile.OpenReadStream();
            var video = new Video();
            video.Title = model.Title;

            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(video.Id, stream),
                Overwrite = true,
                Folder = Folder,
            };

            var result = await this.cloudinary.UploadAsync(uploadParams);
            video.UrlPath = result.Url.ToString();

            video.CourseId = model.CourseId;

            await this.videoRepository.AddAsync(video);
            await this.videoRepository.SaveChangesAsync();
        }
    }
}
