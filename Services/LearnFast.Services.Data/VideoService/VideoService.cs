namespace LearnFast.Services.Data.VideoService
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Services.Mapping.PropertyMatcher;
    using LearnFast.Web.ViewModels.Content;
    using Microsoft.EntityFrameworkCore;

    using Video = LearnFast.Data.Models.Video;

    public class VideoService : IVideoService
    {
        private const string Folder = "videos";

        private readonly Cloudinary cloudinary;
        private readonly IDeletableEntityRepository<Video> videoRepository;
        private readonly IMapper mapper;

        public VideoService(
            IDeletableEntityRepository<Video> videoRepository,
            Cloudinary cloudinary,
            IMapper mapper)
        {
            this.videoRepository = videoRepository;
            this.cloudinary = cloudinary;
            this.mapper = mapper;
        }

        public async Task EditVideo(EditVideoViewModel model)
        {
            var targetVideo = await this.videoRepository.All().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (targetVideo == null)
            {
                throw new ArgumentException(GlobalExceptions.VideoIsNotExistExceptionMessage);
            }

            PropertyCopier<EditVideoViewModel, Video>.CopyPropertiesFrom(model, targetVideo);

            if (model.VideoFile != null)
            {
                var inputModel = this.mapper.Map<ImportVideoModel>(targetVideo);
                inputModel.VideoFile = model.VideoFile;

                var result = await this.RemoteUpload(inputModel, model.Id);
                targetVideo.UrlPath = result.Url.ToString();
            }

            await this.videoRepository.SaveChangesAsync();
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

        public async Task<string> UploadVideo(ImportVideoModel model)
        {
            var video = new Video();
            video.Title = model.Title;
            video.Description = model.Description;

            var result = await this.RemoteUpload(model, video.Id);
            video.UrlPath = result.Url.ToString();

            video.CourseId = model.CourseId;

            await this.videoRepository.AddAsync(video);
            await this.videoRepository.SaveChangesAsync();

            return video.UrlPath;
        }

        private async Task<VideoUploadResult> RemoteUpload(ImportVideoModel model, string videoId)
        {
            using var stream = model.VideoFile.OpenReadStream();

            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(videoId, stream),
                Overwrite = true,
                Folder = Folder,
            };

            return await this.cloudinary.UploadAsync(uploadParams);
        }
    }
}
