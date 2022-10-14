namespace LearnFast.Services.Data.CourseService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Services.Mapping.PropertyMatcher;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.EntityFrameworkCore;

    public class CourseService : ICourseService, ISorterCourse, IFilterCourse
    {
        private readonly IMapper mapper;
        private readonly IDeletableEntityRepository<Course> courseRepository;

        public CourseService(
            IMapper mapper,
            IDeletableEntityRepository<Course> courseRepository)
        {
            this.mapper = mapper;
            this.courseRepository = courseRepository;
        }

        public async Task AddCourseAsync(ImportCourseModel model)
        {
            var course = this.mapper.Map<Course>(model);

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
                throw new ArgumentException(GlobalExceptions.DoNotOwnThisCourseExceptionMessage);
            }

            this.courseRepository.Delete(course);
            await this.courseRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(ImportCourseModel model, string userId, int courseId)
        {
            var course = await this.courseRepository
                .All()
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
            {
                throw new NullReferenceException(GlobalExceptions.CourseIsNotExistExceptionMessage);
            }

            if (userId != course.Owner.Id)
            {
                throw new ArgumentException(GlobalExceptions.DoNotOwnThisCourseExceptionMessage);
            }

            PropertyCopier<ImportCourseModel, Course>.CopyPropertiesFrom(model, course);
            await this.courseRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.GetAllWithBasicInformation().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderByPriceAsync<T>()
        {
            return await this.GetAllWithBasicInformation()
                .OrderBy(x => x.Price)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderByDescendingPriceAsync<T>()
        {
            return await this.GetAllWithBasicInformation()
               .OrderByDescending(x => x.Price)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllBySellsAsync<T>()
        {
            return await this.GetAllWithBasicInformation()
               .OrderByDescending(x => x.CourseStudents)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetFreeCourseAsync<T>()
        {
            return await this.GetAllWithBasicInformation()
               .Where(x => x.IsFree)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetOwnCoursesAsync<T>(string userId)
        {
            return await this.GetAllWithBasicInformation()
               .Where(x => x.Owner.Id == userId)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetEnrolledCoursesAsync<T>(string userId)
        {
            return await this.GetAllWithBasicInformation()
             .Include(x => x.CourseStudents)
             .Where(x => x.CourseStudents.Any(x => x.UserId == userId))
             .To<T>()
             .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetCoursesByLanguageAsync<T>(int languageId)
        {
            return await this.GetAllWithBasicInformation()
               .Where(x => x.LanguageId == languageId)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetCoursesByCategoryAsync<T>(int categoryId)
        {
            return await this.GetAllWithBasicInformation()
               .Where(x => x.CategoryId == categoryId)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetCoursesByDifficultAsync<T>(int difficulty)
        {
            return await this.GetAllWithBasicInformation()
               .Where(x => (int)x.Difficulty == difficulty)
               .To<T>()
               .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int courseId)
        {
            var course = await this.GetWithAllInformation()
                .Where(x => x.Id == courseId)
                .To<T>()
                .FirstOrDefaultAsync();

            if (course == null)
            {
                throw new NullReferenceException(GlobalExceptions.CourseIsNotExistExceptionMessage);
            }

            // Here, should add in map profile the model.
            var model = this.mapper.Map<T>(course);

            return model;
        }

        private IQueryable<Course> GetAllWithBasicInformation()
        {
            return this.courseRepository
               .AllAsNoTracking()
               .Include(x => x.Language)
               .Include(x => x.Category)
               .Include(x => x.Owner);
        }

        private IQueryable<Course> GetWithAllInformation()
        {
            return this.courseRepository
               .AllAsNoTracking()
               .Include(x => x.Language)
               .Include(x => x.Category)
               .Include(x => x.Owner)
               .Include(x => x.Reviews)
               .Include(x => x.CourseStudents)
               .Include(x => x.Content);
        }
    }
}
