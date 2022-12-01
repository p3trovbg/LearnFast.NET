namespace LearnFast.Services.Data.VideoService
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Services.Mapping.PropertyMatcher;
    using LearnFast.Web.ViewModels.Content;
    using Microsoft.EntityFrameworkCore;

    using Video = LearnFast.Data.Models.Video;

    public class VideoService : IVideoService
    {
        private readonly ICloudinaryService cloudinaryService;
        private readonly IDeletableEntityRepository<Video> videoRepository;
        private readonly IMapper mapper;

        public VideoService(
            IDeletableEntityRepository<Video> videoRepository,
            IMapper mapper,
            ICloudinaryService cloudinaryService)
        {
            this.videoRepository = videoRepository;
            this.mapper = mapper;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task EditVideo(EditVideoViewModel model)
        {
            var targetVideo = await this.videoRepository.All().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (targetVideo == null)
            {
                throw new ArgumentException(GlobalExceptions.VideoDoesNotExistExceptionMessage);
            }

            PropertyCopier<EditVideoViewModel, Video>.CopyPropertiesFrom(model, targetVideo);

            if (model.VideoFile != null)
            {
                var inputModel = this.mapper.Map<ImportVideoModel>(targetVideo);
                inputModel.VideoFile = model.VideoFile;

                var result = await this.cloudinaryService.UploadVideoAsync(model.VideoFile, model.Id);

                if (result.Error != null)
                {
                    throw new InvalidOperationException(result.Error.Message);
                }

                targetVideo.UrlPath = result.Url.ToString();
            }

            await this.videoRepository.SaveChangesAsync();
        }

        public async Task RemoveVideo(string videoId)
        {
            var targetVideo = await this.videoRepository.All().FirstOrDefaultAsync(x => x.Id == videoId);

            if (targetVideo == null)
            {
                throw new ArgumentException(GlobalExceptions.VideoDoesNotExistExceptionMessage);
            }

            this.videoRepository.Delete(targetVideo);
            await this.videoRepository.SaveChangesAsync();
        }

        public async Task<string> UploadVideo(ImportVideoModel model)
        {
            var video = new Video();
            video.Title = model.Title;
            video.Description = model.Description;

            var result = await this.cloudinaryService.UploadVideoAsync(model.VideoFile, video.Id);

            if (result.Error != null)
            {
                throw new InvalidOperationException(result.Error.Message);
            }

            video.UrlPath = result.Url.ToString();

            video.CourseId = model.CourseId;

            await this.videoRepository.AddAsync(video);
            await this.videoRepository.SaveChangesAsync();

            return video.UrlPath;
        }
    }
}
