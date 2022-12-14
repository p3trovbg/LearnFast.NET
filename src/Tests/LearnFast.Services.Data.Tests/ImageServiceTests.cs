namespace LearnFast.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using CloudinaryDotNet.Actions;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.ImageService;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class ImageServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Image>> imageRepository;
        private readonly Mock<ICloudinaryService> cloudinaryService;

        public ImageServiceTests()
        {
            this.imageRepository = new Mock<IDeletableEntityRepository<Image>>();
            this.cloudinaryService = new Mock<ICloudinaryService>();
        }

        [Fact]
        public async Task UploadImageShouldReturnImageEntity()
        {
            var expectedImageUrl = "sites/files/images/picture.png";
            this.imageRepository.Setup(x => x.AddAsync(It.IsAny<Image>())).Callback(() => { return; });
            this.imageRepository.Setup(x => x.SaveChangesAsync()).Callback(() => { return; });

            this.cloudinaryService
                .Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync(new ImageUploadResult { Url = new Uri(expectedImageUrl, UriKind.Relative) });

            var service = new ImageService(this.imageRepository.Object, this.cloudinaryService.Object);

            var file = new Mock<IFormFile>();

            var result = await service.UploadImage(file.Object, GlobalConstants.ImagesFolderName);

            Assert.Equal(expectedImageUrl, result.UrlPath);
            this.imageRepository.Verify(m => m.SaveChangesAsync(), Times.Once());
            this.imageRepository.Verify(m => m.AddAsync(It.IsAny<Image>()), Times.Once());
        }

        [Fact]
        public async Task UploadImageShouldThrowsExceptionIfCloudinaryReturnError()
        {
            var expectedException = "invalid upload";
            this.imageRepository.Setup(x => x.AddAsync(It.IsAny<Image>())).Callback(() => { return; });
            this.imageRepository.Setup(x => x.SaveChangesAsync()).Callback(() => { return; });

            this.cloudinaryService
                .Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync(new ImageUploadResult { Error = new Error() { Message = expectedException } });

            var service = new ImageService(this.imageRepository.Object, this.cloudinaryService.Object);

            var file = new Mock<IFormFile>();

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.UploadImage(file.Object, GlobalConstants.ImagesFolderName));
            Assert.Equal(expectedException, ex.Message);
        }
    }
}
