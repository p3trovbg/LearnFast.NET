namespace LearnFast.Services.Data.ReviewService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using CloudinaryDotNet;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Services.Mapping;
    using LearnFast.Services.Mapping.PropertyMatcher;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Review;
    using Microsoft.EntityFrameworkCore;

    public class ReviewService : IReviewService
    {
        private readonly IDeletableEntityRepository<Review> reviewRepository;
        private readonly IFilterCourse filterCourse;
        private readonly IMapper mapper;

        public ReviewService(
            IDeletableEntityRepository<Review> reviewRepository,
            IFilterCourse filterCourse,
            IMapper mapper)
        {
            this.reviewRepository = reviewRepository;
            this.filterCourse = filterCourse;
            this.mapper = mapper;
        }

        public async Task Add(ImportReviewViewModel model)
        {
            var course = await this.filterCourse.GetByIdAsync<BaseCourseViewModel>(model.CourseId);

            if (course == null)
            {
                throw new ArgumentException(GlobalExceptions.CourseIsNotExistExceptionMessage);
            }

            var review = this.mapper.Map<Review>(model);

            await this.reviewRepository.AddAsync(review);
            await this.reviewRepository.SaveChangesAsync();
        }

        public async Task Delete(int reviewId, string userId)
        {
            var currentReview = await this.reviewRepository.All().Include(x => x.User).Where(x => x.Id == reviewId).FirstOrDefaultAsync();


            if (currentReview == null)
            {
                throw new ArgumentException(GlobalExceptions.DoesNotExistReview);
            }

            if (currentReview.User.Id != userId)
            {
                throw new ArgumentException(GlobalExceptions.UserNotHasPermission);
            }

            this.reviewRepository.Delete(currentReview);
            await this.reviewRepository.SaveChangesAsync();
        }

        public async Task Edit(EditReviewViewModel model)
        {
            var review = await this.reviewRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (review == null)
            {
                throw new ArgumentException(GlobalExceptions.DoesNotExistReview);
            }

            PropertyCopier<EditReviewViewModel, Review>.CopyPropertiesFrom(model, review);

            await this.reviewRepository.SaveChangesAsync();
        }

        public async Task GetAllReviewsByCourse(ReviewListViewModel model)
        {
            var course = await this.filterCourse.GetByIdAsync<BaseCourseViewModel>(model.CourseId);

            if (course == null)
            {
                throw new ArgumentException(GlobalExceptions.CourseIsNotExistExceptionMessage);
            }

            var reviews = this.reviewRepository
                .AllAsNoTracking()
                .Where(x => x.CourseId == model.CourseId);

            int reviewsCount = await reviews.CountAsync();

            model.Page = model.Page == null ? 1 : model.Page;
            model.TotalCount = reviewsCount;
            model.Reviews = await reviews
            .OrderByDescending(x => x.CreatedOn)
            .Skip((int)((model.Page - 1) * model.ItemsPerPage))
            .Take(model.ItemsPerPage)
            .To<ReviewViewModel>()
            .ToListAsync();
        }

        public async Task<T> GetReviewById<T>(int reviewId)
        {
            return await this.reviewRepository.AllAsNoTracking().Where(x => x.Id == reviewId).To<T>().FirstOrDefaultAsync();
        }

        public async Task<int> GetReviewsCountAsync()
        {
            return await this.reviewRepository.AllAsNoTracking().CountAsync();
        }

        public async Task<IEnumerable<T>> GetSelectedReviewsByCourse<T>(int courseId)
        {
            return await this.reviewRepository.AllAsNoTracking()
                .Where(x => x.CourseId == courseId && x.IsSelected)
                .To<T>()
                .ToListAsync();
        }

        public async Task Selecting(SelectingReviewViewModel model)
        {
            var selectedReviews = this.reviewRepository.All().Where(x => x.CourseId == model.CourseId);

            if (await selectedReviews.CountAsync() == 5)
            {
                throw new ArgumentOutOfRangeException(GlobalExceptions.LimitOfSelectedReviews);
            }

            var review = await selectedReviews.Where(x => x.Id == model.ReviewId).FirstOrDefaultAsync();

            if (review == null)
            {
                throw new ArgumentException(GlobalExceptions.DoesNotExistReview);
            }

            if (model.IsSelected)
            {
                review.IsSelected = false;
            }
            else
            {
                review.IsSelected = true;
            }

            await this.reviewRepository.SaveChangesAsync();
        }
    }
}
