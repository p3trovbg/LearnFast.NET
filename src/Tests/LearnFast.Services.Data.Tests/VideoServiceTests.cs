namespace LearnFast.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet.Actions;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.VideoService;
    using LearnFast.Web.ViewModels.Content;
    using Microsoft.AspNetCore.Http;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;
    using Video = LearnFast.Data.Models.Video;

    public class VideoServiceTests : BaseServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Video>> videoRepository;
        private readonly Mock<ICloudinaryService> cloudinaryService;

        public VideoServiceTests()
        {
            this.videoRepository = new Mock<IDeletableEntityRepository<Video>>();
            this.cloudinaryService = new Mock<ICloudinaryService>();
        }

        [Fact]
        public async Task UploadVideoShouldReturnVideoUrlAsString()
        {
            var expectedVideoUrl = "sites/files/images/picture.png";
            this.videoRepository.Setup(x => x.AddAsync(It.IsAny<Video>())).Callback(() => { return; });
            this.videoRepository.Setup(x => x.SaveChangesAsync()).Callback(() => { return; });

            this.cloudinaryService
                .Setup(x => x.UploadVideoAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync(new VideoUploadResult { Url = new Uri(expectedVideoUrl, UriKind.Relative) });

            var service = new VideoService(this.videoRepository.Object, this.Mapper, this.cloudinaryService.Object);

            var file = new Mock<IFormFile>();

            var videoModel = new ImportVideoModel() { VideoFile = file.Object, Title = "Introduction", CourseId = 5, Description = "test" };

            var result = await service.UploadVideo(videoModel);

            Assert.Equal(expectedVideoUrl, result);
            this.videoRepository.Verify(m => m.SaveChangesAsync(), Times.Once());
            this.videoRepository.Verify(m => m.AddAsync(It.IsAny<Video>()), Times.Once());
        }

        [Fact]
        public async Task UploadVideoShouldThrowsExceptionIfCloudinaryReturnError()
        {
            var expectedException = "testException";
            this.videoRepository.Setup(x => x.AddAsync(It.IsAny<Video>())).Callback(() => { return; });
            this.videoRepository.Setup(x => x.SaveChangesAsync()).Callback(() => { return; });

            this.cloudinaryService
                .Setup(x => x.UploadVideoAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
            .ReturnsAsync(new VideoUploadResult { Error = new Error() { Message = expectedException } });

            var service = new VideoService(this.videoRepository.Object, this.Mapper, this.cloudinaryService.Object);

            var file = new Mock<IFormFile>();

            var videoModel = new ImportVideoModel() { VideoFile = file.Object, Title = "Introduction", CourseId = 5, Description = "test" };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.UploadVideo(videoModel));
            Assert.Equal(expectedException, ex.Message);
        }

        [Fact]
        public async Task RemoveVideoShouldRemovesTargetVideo()
        {
            var videos = GetVideos();

            var targetVideo = videos.FirstOrDefault();

            this.videoRepository.Setup(x => x.SaveChangesAsync()).Callback(() => { return; });
            this.videoRepository.Setup(x => x.All()).Returns(videos.BuildMock());
            this.videoRepository.Setup(x => x.Delete(It.IsAny<Video>())).Callback(() => { targetVideo.IsDeleted = true; });

            var service = new VideoService(this.videoRepository.Object, this.Mapper, this.cloudinaryService.Object);

            await service.RemoveVideo(targetVideo.Id);
            Assert.True(targetVideo.IsDeleted);
            this.videoRepository.Verify(m => m.Delete(It.IsAny<Video>()), Times.Once());
            this.videoRepository.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task RemoveVideoShouldThrowsExceptionIfVideoDoesNotExist()
        {
            var videos = GetVideos();

            this.videoRepository.Setup(x => x.All()).Returns(videos.BuildMock());

            var service = new VideoService(this.videoRepository.Object, this.Mapper, this.cloudinaryService.Object);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.RemoveVideo("1"));
            Assert.Equal(GlobalExceptions.VideoDoesNotExistExceptionMessage, ex.Message);

            this.videoRepository.Verify(m => m.All(), Times.Once());
        }

        [Fact]
        public async Task EditVideoShouldUpdatesAllPropertiesThatIsChanged()
        {
            var videos = GetVideos();

            var targetVideo = videos.FirstOrDefault();

            var expectedUpdatedVideoUrl = "sites/files/images/updatedVideo.png";
            this.videoRepository.Setup(x => x.AddAsync(It.IsAny<Video>())).Callback(() => { return; });
            this.videoRepository.Setup(x => x.All()).Returns(videos.BuildMock());
            this.videoRepository.Setup(x => x.SaveChangesAsync()).Callback(() => { return; });

            this.cloudinaryService
                .Setup(x => x.UploadVideoAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync(new VideoUploadResult { Url = new Uri(expectedUpdatedVideoUrl, UriKind.Relative) });

            var service = new VideoService(this.videoRepository.Object, this.Mapper, this.cloudinaryService.Object);

            var file = new Mock<IFormFile>();

            var videoModel = new EditVideoViewModel()
            { VideoFile = file.Object, Title = "Introduction", CourseId = 5, Description = "test", Id = targetVideo.Id };

            await service.EditVideo(videoModel);

            Assert.Equal(expectedUpdatedVideoUrl, targetVideo.UrlPath);
        }

        [Fact]
        public async Task EditVideoShouldThrowsExceptionIfCloudinaryReturnsError()
        {
            var videos = GetVideos();

            var targetVideo = videos.FirstOrDefault();
            var expectedException = "exception";

            this.videoRepository.Setup(x => x.All()).Returns(videos.BuildMock());

            this.cloudinaryService
                .Setup(x => x.UploadVideoAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
            .ReturnsAsync(new VideoUploadResult { Error = new Error() { Message = expectedException } });

            var service = new VideoService(this.videoRepository.Object, this.Mapper, this.cloudinaryService.Object);

            var file = new Mock<IFormFile>();

            var videoModel = new EditVideoViewModel()
            { VideoFile = file.Object, Title = "Introduction", CourseId = 5, Description = "test", Id = targetVideo.Id };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.EditVideo(videoModel));
            Assert.Equal(expectedException, ex.Message);
        }

        [Fact]
        public async Task EditVideoShouldThrowsExceptionIfVideoDoesNotExist()
        {
            var videos = GetVideos();

            this.videoRepository.Setup(x => x.All()).Returns(videos.BuildMock());

            var service = new VideoService(this.videoRepository.Object, this.Mapper, this.cloudinaryService.Object);
            var file = new Mock<IFormFile>();

            var videoModel = new EditVideoViewModel()
            { VideoFile = file.Object, Title = "Introduction", CourseId = 5, Description = "test", Id = "1" };

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.EditVideo(videoModel));
            Assert.Equal(GlobalExceptions.VideoDoesNotExistExceptionMessage, ex.Message);
        }

        private List<Video> GetVideos()
        {
            return new List<Video>()
            {
                new Video() { CourseId = 5, Description = "test", Title = "test1", UrlPath = "test" },
                new Video() { CourseId = 5, Description = "test", Title = "test2" },
                new Video() { CourseId = 2, Description = "test", Title = "test3" },
            };
        }
    }
}
