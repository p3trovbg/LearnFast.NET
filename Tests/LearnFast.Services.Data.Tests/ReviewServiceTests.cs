namespace LearnFast.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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

        [Fact]
        public async Task DeleteReview()
        {
            var reviews = GetReviews();
            var targetReview = reviews.FirstOrDefault(x => x.Id == 1);

            this.repository.Setup(x => x.All()).Returns(reviews.AsQueryable().BuildMock());
            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            await service.Delete(targetReview.Id, targetReview.User.Id);

            this.repository.Verify(m => m.Delete(It.IsAny<Review>()), Times.Once());
            this.repository.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task DeleteNotExistReviewShouldThrowException()
        {
            var reviews = GetReviews();
            var targetReview = reviews.FirstOrDefault(x => x.Id == 1);

            this.repository.Setup(x => x.All()).Returns(reviews.AsQueryable().BuildMock());
            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.Delete(-1, targetReview.User.Id));
            Assert.Equal(GlobalExceptions.DoesNotExistReview, ex.Message);
        }

        [Fact]
        public async Task DeleteReviewByUserThatIsNotOwnerShouldThrowException()
        {
            var reviews = GetReviews();
            var targetReview = reviews.FirstOrDefault(x => x.Id == 1);

            this.repository.Setup(x => x.All()).Returns(reviews.AsQueryable().BuildMock());
            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.Delete(targetReview.Id, "testId"));
            Assert.Equal(GlobalExceptions.UserNotHasPermission, ex.Message);
        }

        [Fact]
        public async Task EditReviewShouldUpdatePropoertiesThatIsChanged()
        {
            var reviews = GetReviews();
            var targetReview = new EditReviewViewModel { Id = 1, Content = "TestUpdate", Rating = 6 };

            this.repository.Setup(x => x.All()).Returns(reviews.AsQueryable().BuildMock());

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            await service.Edit(targetReview);

            var updatedReview = reviews.FirstOrDefault(x => x.Id == 1);

            Assert.Equal(targetReview.Content, updatedReview.Content);
            Assert.Equal(targetReview.Rating, updatedReview.Rating);
            this.repository.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task EditReviewShouldThrowsExceptionIfReviewIdIsInvalid()
        {
            var targetReview = new EditReviewViewModel { Id = -1, Content = "TestUpdate", Rating = 6 };
            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.Edit(targetReview));
            Assert.Equal(GlobalExceptions.DoesNotExistReview, ex.Message);
        }

        [Theory]
        [InlineData(5)]
        public async Task GetAllReviewsByCourseShouldSelectsReviewsByValidCourse(int courseId)
        {
            var allReviews = GetReviews();
            var model = new ReviewListViewModel();

            var mappedReviews = this.Mapper.Map<List<ReviewViewModel>>(allReviews);

            this.filterCourse
              .Setup(x => x.GetCourseByIdAsync<BaseCourseViewModel>(It.IsAny<int>())).ReturnsAsync(new BaseCourseViewModel { Id = courseId });

            model.Reviews = mappedReviews;
            model.CourseId = courseId;

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            await service.GetAllReviewsByCourse(model);

            Assert.All(model.Reviews, x => Assert.Equal(courseId, x.CourseId));
        }

        [Theory]
        [InlineData(5)]
        public async Task GetAllReviewsByCourseShouldReturnOnly9ReviewsByPage(int courseId)
        {
            var allReviews = GetReviews();
            var model = new ReviewListViewModel();

            var mappedReviews = this.Mapper.Map<List<ReviewViewModel>>(allReviews);

            this.filterCourse
              .Setup(x => x.GetCourseByIdAsync<BaseCourseViewModel>(It.IsAny<int>())).ReturnsAsync(new BaseCourseViewModel { Id = courseId });

            model.Reviews = mappedReviews;
            model.CourseId = courseId;

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            await service.GetAllReviewsByCourse(model);

            Assert.Equal(9, model.Reviews.Count());
            Assert.All(model.Reviews, x => Assert.Equal(courseId, x.CourseId));
        }

        [Theory]
        [InlineData(-1)]
        public async Task GetAllReviewsByCourseShouldThrowsExceptionIfCourseDoesNotExist(int courseId)
        {
            var model = new ReviewListViewModel() { CourseId = courseId };

            this.filterCourse
              .Setup(x => x.GetCourseByIdAsync<BaseCourseViewModel>(It.IsAny<int>())).ReturnsAsync((BaseCourseViewModel)null);

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetAllReviewsByCourse(model));
            Assert.Equal(GlobalExceptions.CourseDoesNotExistExceptionMessage, ex.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public async Task GetReviewByIdShouldReturnReview(int reviewId)
        {
            var expectedReview = GetReviews().FirstOrDefault(x => x.Id == reviewId);

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            var result = await service.GetReviewById<ReviewViewModel>(reviewId);

            Assert.Equal(expectedReview.Id, result.Id);
            Assert.Equal(expectedReview.Content, result.Content);
        }

        [Theory]
        [InlineData(-50)]
        [InlineData(-1)]
        public async Task GetReviewByInvalidIdShouldThrowsException(int reviewId)
        {
            var expectedReview = GetReviews().FirstOrDefault(x => x.Id == reviewId);

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            var ex = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.GetReviewById<ReviewViewModel>(reviewId));
            Assert.Equal(GlobalExceptions.DoesNotExistReview, ex.Message);
        }

        [Fact]
        public async Task GetReviewsCountShouldReturnCorrectNumber()
        {
            var expectedReviewsCount = GetReviews().Count();
            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            var result = await service.GetReviewsCountAsync();
            Assert.Equal(expectedReviewsCount, result);
            this.repository.Verify(m => m.AllAsNoTracking(), Times.Once());
        }

        [Theory]
        [InlineData(5)]
        [InlineData(3)]
        public async Task GetSelectedReviewByCourseShouldReturnSelectedReviews(int courseId)
        {
            this.filterCourse
              .Setup(x => x.GetCourseByIdAsync<BaseCourseViewModel>(It.IsAny<int>()))
              .ReturnsAsync(new BaseCourseViewModel { Id = courseId });

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            var result = await service.GetSelectedReviewsByCourse<ReviewViewModel>(courseId);

            Assert.All(result, x => Assert.True(x.IsSelected));
            this.repository.Verify(m => m.AllAsNoTracking(), Times.Once());
        }

        [Theory]
        [InlineData(5)]
        [InlineData(3)]
        public async Task GetSelectedReviewByInvalidCourseShouldThrowsException(int courseId)
        {
            this.filterCourse
              .Setup(x => x.GetCourseByIdAsync<BaseCourseViewModel>(It.IsAny<int>()))
              .ReturnsAsync((BaseCourseViewModel)null);

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetSelectedReviewsByCourse<ReviewViewModel>(courseId));
            Assert.Equal(GlobalExceptions.CourseDoesNotExistExceptionMessage, ex.Message);
        }

        [Theory]
        [InlineData(4)]
        public async Task SelectingReviewByCourseShouldMakesIsSelectedTrueIfIsFalse(int reviewId)
        {
            var reviews = GetReviews();
            var expected = reviews.FirstOrDefault(x => x.Id == reviewId);
            var model = new SelectingReviewViewModel() { CourseId = expected.CourseId, ReviewId = reviewId, IsSelected = expected.IsSelected };

            this.repository.Setup(x => x.All()).Returns(reviews.BuildMock());

            this.filterCourse
               .Setup(x => x.GetCourseByIdAsync<BaseCourseViewModel>(It.IsAny<int>()))
               .ReturnsAsync(new BaseCourseViewModel { Id = expected.CourseId });

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);
            await service.Selecting(model);

            Assert.True(expected.IsSelected);
            this.repository.Verify(m => m.All(), Times.Once());
            this.repository.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [Theory]
        [InlineData(14)]
        public async Task SelectingReviewByCourseShouldMakesIsSelectedFalseIfIsTrue(int reviewId)
        {
            var reviews = GetReviews();
            var expected = reviews.FirstOrDefault(x => x.Id == reviewId);
            var model = new SelectingReviewViewModel() { CourseId = expected.CourseId, ReviewId = reviewId, IsSelected = expected.IsSelected };

            this.repository.Setup(x => x.All()).Returns(reviews.BuildMock());

            this.filterCourse
               .Setup(x => x.GetCourseByIdAsync<BaseCourseViewModel>(It.IsAny<int>()))
               .ReturnsAsync(new BaseCourseViewModel { Id = expected.CourseId });

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);
            await service.Selecting(model);

            Assert.False(expected.IsSelected);
            this.repository.Verify(m => m.All(), Times.Once());
            this.repository.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task SelectingReviewByCouseDoesNotExistShouldThrowsException()
        {
            this.filterCourse
               .Setup(x => x.GetCourseByIdAsync<BaseCourseViewModel>(It.IsAny<int>()))
               .ReturnsAsync((BaseCourseViewModel)null);

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);
            var model = new SelectingReviewViewModel();

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.Selecting(model));
            Assert.Equal(GlobalExceptions.CourseDoesNotExistExceptionMessage, ex.Message);
        }

        [Theory]
        [InlineData(10)]
        public async Task SelectingReviewByCourseShouldThrowsExceptionIfSelectedReviewsAreMoreThan5(int reviewId)
        {
            var reviews = GetReviews().Where(x => x.CourseId == 5);
            var expected = reviews.FirstOrDefault(x => x.Id == reviewId);
            var model = new SelectingReviewViewModel() { CourseId = expected.CourseId, ReviewId = reviewId, IsSelected = expected.IsSelected };

            this.repository.Setup(x => x.All()).Returns(reviews.BuildMock());

            this.filterCourse
               .Setup(x => x.GetCourseByIdAsync<BaseCourseViewModel>(It.IsAny<int>()))
               .ReturnsAsync(new BaseCourseViewModel { Id = expected.CourseId });

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await service.Selecting(model));
            Assert.Equal(GlobalExceptions.LimitOfSelectedReviews, ex.ParamName);
        }

        [Theory]
        [InlineData(-1, 5)]
        public async Task SelectingReviewByCourseShouldThrowsExceptionIfReviewDoesNotExist(int reviewId, int courseId)
        {
            var reviews = GetReviews().Where(x => x.CourseId == courseId);
            var model = new SelectingReviewViewModel() { CourseId = courseId, ReviewId = reviewId };

            this.repository.Setup(x => x.All()).Returns(reviews.BuildMock());

            this.filterCourse
               .Setup(x => x.GetCourseByIdAsync<BaseCourseViewModel>(It.IsAny<int>()))
               .ReturnsAsync(new BaseCourseViewModel { Id = courseId });

            var service = new ReviewService(this.repository.Object, this.filterCourse.Object, this.Mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.Selecting(model));
            Assert.Equal(GlobalExceptions.DoesNotExistReview, ex.Message);
        }

        private void Setup()
        {
            this.repository.Setup(x => x.AddAsync(It.IsAny<Review>())).Callback(() => { return; });
            this.repository.Setup(x => x.Delete(It.IsAny<Review>())).Callback(() => { return; });
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
                new Review { Id = 1, Content = "Test", CourseId = 1, Rating = 5, User = new ApplicationUser() },
                new Review { Id = 2, Content = "Test", CourseId = 1, Rating = 3, User = new ApplicationUser() },
                new Review { Id = 3, Content = "Test", CourseId = 2, Rating = 4, User = new ApplicationUser() },
                new Review { Id = 4, Content = "Test", CourseId = 3, Rating = 6, User = new ApplicationUser() },
                new Review { Id = 5, Content = "Test", CourseId = 4, Rating = 2, User = new ApplicationUser() },
                new Review { Id = 6, Content = "Test", CourseId = 4, Rating = 1, User = new ApplicationUser() },
                new Review { Id = 7, Content = "SELECTED", CourseId = 5, Rating = 6, IsSelected = false, User = new ApplicationUser() },
                new Review { Id = 8, Content = "SELECTED", CourseId = 5, Rating = 6, IsSelected = false, User = new ApplicationUser() },
                new Review { Id = 9, Content = "SELECTED", CourseId = 5, Rating = 6, IsSelected = false, User = new ApplicationUser() },
                new Review { Id = 10, Content = "SELECTED", CourseId = 5, Rating = 6, IsSelected = false, User = new ApplicationUser() },
                new Review { Id = 11, Content = "SELECTED", CourseId = 5, Rating = 6, IsSelected = false, User = new ApplicationUser() },
                new Review { Id = 12, Content = "SELECTED", CourseId = 5, Rating = 6, IsSelected = false, User = new ApplicationUser() },
                new Review { Id = 13, Content = "SELECTED", CourseId = 5, Rating = 6, IsSelected = true, User = new ApplicationUser() },
                new Review { Id = 14, Content = "SELECTED", CourseId = 5, Rating = 6, IsSelected = true, User = new ApplicationUser() },
                new Review { Id = 15, Content = "SELECTED", CourseId = 5, Rating = 6, IsSelected = true, User = new ApplicationUser() },
                new Review { Id = 17, Content = "SELECTED", CourseId = 5, Rating = 6, IsSelected = true, User = new ApplicationUser() },
                new Review { Id = 18, Content = "SELECTED", CourseId = 5, Rating = 6, IsSelected = true, User = new ApplicationUser() },
                new Review { Id = 19, Content = "SELECTED", CourseId = 5, Rating = 6, IsSelected = true, User = new ApplicationUser() },
            };
        }
    }
}
