namespace LearnFast.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Services.Data.ReviewService;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Review;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class ReviewServiceTests : BaseServiceTests
    {
        private Mock<IDeletableEntityRepository<Review>> repository;
        private Mock<IFilterCourse> filterCourse;

        public ReviewServiceTests()
        {
            this.repository = new Mock<IDeletableEntityRepository<Review>>();
            this.filterCourse = new Mock<IFilterCourse>();
            this.Setup();
        }

        [Fact]
        public async Task AddReviewShouldInNotExistCourseShouldThrowsException()
        {
            var reviews = GetReviews();

            var newReview = new ImportReviewViewModel() { Content = "test", CourseId = 5, Rating = 5, UserId = "test" };

            this.filterCourse
              .Setup(x => x.GetCourseByIdAsync<BaseCourseViewModel>(It.IsAny<int>())).ReturnsAsync((BaseCourseViewModel)null);

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.Add(newReview));
            Assert.Equal(GlobalExceptions.CourseDoesNotExistExceptionMessage, ex.Message);
        }

        [Fact]
        public async Task AddReviewShouldAddReviewToCurrentCourse()
        {
            var reviews = GetReviews();

            var newReview = new ImportReviewViewModel() { Content = "test", CourseId = 5, Rating = 5, UserId = "test" };

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            await service.Add(newReview);

            this.repository.Verify(m => m.AddAsync(It.IsAny<Review>()), Times.Once());
            this.repository.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        private void Setup()
        {
            this.repository.Setup(x => x.AddAsync(It.IsAny<Review>())).Callback(() => { return; });
            this.repository.Setup(x => x.SaveChangesAsync()).Callback(() => { return; });
            this.repository.Setup(x => x.All()).Returns(GetReviews().AsQueryable().BuildMock());
            this.repository.Setup(x => x.AllAsNoTracking()).Returns(GetReviews().AsQueryable().BuildMock());

            this.filterCourse
                .Setup(x => x.GetCourseByIdAsync<BaseCourseViewModel>(It.IsAny<int>())).ReturnsAsync(new BaseCourseViewModel { Id = 1 });
        }

        private static List<Review> GetReviews()
        {
            return new List<Review>()
            {
                new Review { Content = "Test", CourseId = 1, Rating = 5, UserId = "test1" },
                new Review { Content = "Test", CourseId = 1, Rating = 3, UserId = "test2" },
                new Review { Content = "Test", CourseId = 2, Rating = 4, UserId = "test3" },
                new Review { Content = "Test", CourseId = 3, Rating = 6, UserId = "test5" },
                new Review { Content = "Test", CourseId = 4, Rating = 2, UserId = "test5" },
                new Review { Content = "Test", CourseId = 4, Rating = 1, UserId = "test6" },
                new Review { Content = "SELECTED", CourseId = 5, Rating = 6, UserId = "test7", IsSelected = true },
                new Review { Content = "SELECTED", CourseId = 5, Rating = 6, UserId = "test8" , IsSelected = true },
            };
        }
    }
}
