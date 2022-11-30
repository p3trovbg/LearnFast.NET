
using LearnFast.Data.Common.Repositories;
using LearnFast.Data.Models;
using LearnFast.Services.Data.ReviewService;
using LearnFast.Web.ViewModels.Review;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace LearnFast.Services.Data.Tests
{
    public class ReviewServiceTests : BaseServiceTests
    {
        private Mock<IDeletableEntityRepository<Review>> repository;

        public ReviewServiceTests()
        {
            this.repository = new Mock<IDeletableEntityRepository<Review>>();
        }

        [Fact]
        public async Task AddReview()
        {
            var review = new ImportReviewViewModel
            {
                Content = "This course is very cool!",
                UserId = "user123",
                CourseId = 2,
                Rating = 6,
            };

            this.repository.Setup(x => x.AddAsync(It.IsAny<Review>())).Returns(() => { return; });
            this.repository.Setup(x => x.SaveChangesAsync()).Returns(() => { return; });

            var service = new ReviewService(this.repository.Object, null, this.Mapper);

            await service.Add(review);

        }
    }
}
