﻿namespace LearnFast.Services.Data.CourseService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Data.Models.Enums;
    using LearnFast.Services.Data.ImageService;
    using LearnFast.Services.Mapping;
    using LearnFast.Services.Mapping.PropertyMatcher;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.EntityFrameworkCore;

    public class CourseService : ICourseService, ISorterCourse, IFilterCourse
    {
        private const string BaseCourseImageUrl = "https://akm-img-a-in.tosshub.com/indiatoday/images/bodyeditor/202009/e-learning_digital_education-1200x1080.jpg?XjMNHsb4gLoU_cC7110HB7jVghJQROOj";
        private readonly IMapper mapper;
        private readonly IImageService imageService;
        private readonly IDeletableEntityRepository<Course> courseRepository;

        public CourseService(
            IMapper mapper,
            IDeletableEntityRepository<Course> courseRepository,
            IImageService imageService)
        {
            this.mapper = mapper;
            this.courseRepository = courseRepository;
            this.imageService = imageService;
        }

        public async Task AddCourseAsync(ImportCourseModel model)
        {
            var course = this.mapper.Map<Course>(model);

            if (model.MainImage != null)
            {
                var image = await this.imageService.UploadImage(model.MainImage, "images");
                course.MainImageUrl = image.UrlPath;
            }
            else
            {
                course.MainImageUrl = BaseCourseImageUrl;
            }

            _ = course.Price == 0 ? course.IsFree = true : course.IsFree = false;

            await this.courseRepository.AddAsync(course);
            await this.courseRepository.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await this.courseRepository.AllAsNoTracking().CountAsync();
        }

        public async Task DeleteCourseByIdAsync(int courseId, string userId)
        {
            var course = await this.courseRepository
                .All()
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
            {
                throw new NullReferenceException(GlobalExceptions.CourseIsNotExistExceptionMessage);
            }

            if (course.Owner.Id != userId)
            {
                throw new ArgumentException(GlobalExceptions.DoesNotOwnThisCourseExceptionMessage);
            }

            this.courseRepository.Delete(course);
            await this.courseRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(ImportCourseModel model, string userId)
        {
            var course = await this.courseRepository
                .All()
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (course == null)
            {
                throw new NullReferenceException(GlobalExceptions.CourseIsNotExistExceptionMessage);
            }

            if (userId != course.Owner.Id)
            {
                throw new ArgumentException(GlobalExceptions.DoesNotOwnThisCourseExceptionMessage);
            }

            PropertyCopier<ImportCourseModel, Course>.CopyPropertiesFrom(model, course);

            if (model.MainImage != null)
            {
                var image = await this.imageService.UploadImage(model.MainImage, "images");
                course.MainImageUrl = image.UrlPath;
            }

            _ = course.Price == 0 ? course.IsFree = true : course.IsFree = false;
            course.Difficulty = (Difficulty)model.Difficulty;
            await this.courseRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.GetAllWithBasicInformationAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderByPriceAsync<T>()
        {
            return await this.GetAllWithBasicInformationAsNoTracking()
                .OrderBy(x => x.Price)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderByDescendingPriceAsync<T>()
        {
            return await this.GetAllWithBasicInformationAsNoTracking()
               .OrderByDescending(x => x.Price)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllBySellsAsync<T>()
        {
            return await this.GetAllWithBasicInformationAsNoTracking()
               .OrderByDescending(x => x.CourseStudents)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetFreeCourseAsync<T>()
        {
            return await this.GetAllWithBasicInformationAsNoTracking()
               .Where(x => x.IsFree)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetOwnCoursesAsync<T>(string userId)
        {
            return await this.GetAllWithBasicInformationAsNoTracking()
               .Where(x => x.Owner.Id == userId)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetEnrolledCoursesAsync<T>(string userId)
        {
            return await this.GetAllWithBasicInformationAsNoTracking()
             .Include(x => x.CourseStudents)
             .Where(x => x.CourseStudents.Any(x => x.UserId == userId))
             .To<T>()
             .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetCoursesByLanguageAsync<T>(int languageId)
        {
            return await this.GetAllWithBasicInformationAsNoTracking()
               .Where(x => x.LanguageId == languageId)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetCoursesByCategoryAsync<T>(int categoryId)
        {
            return await this.GetAllWithBasicInformationAsNoTracking()
               .Where(x => x.CategoryId == categoryId)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetCoursesByDifficultAsync<T>(int difficulty)
        {
            return await this.GetAllWithBasicInformationAsNoTracking()
               .Where(x => (int)x.Difficulty == difficulty)
               .To<T>()
               .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int courseId)
        {
            var course = await this.GetWithAllInformationAsNoTracking()
                .Where(x => x.Id == courseId)
                .To<T>()
                .FirstOrDefaultAsync();

            if (course == null)
            {
                throw new NullReferenceException(GlobalExceptions.CourseIsNotExistExceptionMessage);
            }

            var model = this.mapper.Map<T>(course);

            return model;
        }

        private IQueryable<Course> GetAllWithBasicInformationAsNoTracking()
        {
            return this.courseRepository
               .AllAsNoTracking()
               .Include(x => x.Language)
               .Include(x => x.Category)
               .Include(x => x.Owner);
        }

        private IQueryable<Course> GetWithAllInformationAsNoTracking()
        {
            return this.courseRepository
               .AllAsNoTracking()
               .Include(x => x.Language)
               .Include(x => x.Category)
               .Include(x => x.Owner)
               .Include(x => x.Reviews)
               .Include(x => x.Videos)
               .Include(x => x.Images);
        }
    }
}
