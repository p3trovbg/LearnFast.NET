namespace LearnFast.Services.Data.ReviewService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Services.Mapping;
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

        public async Task<IEnumerable<T>> GetAllReviewsByCourse<T>(int courseId)
        {
            var course = await this.filterCourse.GetByIdAsync<BaseCourseViewModel>(courseId);

            if (course == null)
            {
                throw new ArgumentException(GlobalExceptions.CourseIsNotExistExceptionMessage);
            }

            return await this.reviewRepository.AllAsNoTracking()
                .Where(x => x.CourseId == courseId)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetSelectedReviewsByCourse<T>(int courseId)
        {
            return await this.reviewRepository.AllAsNoTracking()
                .Where(x => x.CourseId == courseId && x.IsSelected)
                .To<T>()
                .ToListAsync();
        }
    }
}
