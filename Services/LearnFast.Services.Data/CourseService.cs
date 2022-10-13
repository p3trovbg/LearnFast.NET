namespace LearnFast.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
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
                throw new InvalidOperationException("No such course exists!");
            }

            if (course.Owner.Id != userId)
            {
                throw new ArgumentException("This user is not in possession of this course!");
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
                throw new ArgumentException("Don't exist this course.");
            }

            if (userId != course.Owner.Id)
            {
                throw new ArgumentException("This user is not in possession of this course!");
            }

            PropertyCopier<ImportCourseModel, Course>.CopyPropertiesFrom(model, course);
            await this.courseRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.courseRepository
                .AllAsNoTracking()
                .Include(x => x.Language)
                .Include(x => x.Category)
                .Include(x => x.Owner)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderByPriceAsync<T>()
        {
            return await this.courseRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Price)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderByDescendingPriceAsync<T>()
        {
            return await this.courseRepository
               .AllAsNoTracking()
               .OrderByDescending(x => x.Price)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllBySellsAsync<T>()
        {
            return await this.courseRepository
               .AllAsNoTracking()
               .Include(x => x.CourseStudents)
               .OrderByDescending(x => x.CourseStudents)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetFreeCourseAsync<T>()
        {
            return await this.courseRepository
               .AllAsNoTracking()
               .Where(x => x.IsFree)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetOwnCoursesAsync<T>(string userId)
        {
            return await this.courseRepository
               .AllAsNoTracking()
               .Include(x => x.Owner)
               .Where(x => x.Owner.Id == userId)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetEnrolledCoursesAsync<T>(string userId)
        {
            return await this.courseRepository
             .AllAsNoTracking()
             .Include(x => x.CourseStudents)
             .Where(x => x.CourseStudents.Any(x => x.UserId == userId))
             .To<T>()
             .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetCoursesByLanguageAsync<T>(int languageId)
        {
            return await this.courseRepository
               .AllAsNoTracking()
               .Where(x => x.LanguageId == languageId)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetCoursesByCategoryAsync<T>(int categoryId)
        {
            return await this.courseRepository
               .AllAsNoTracking()
               .Where(x => x.CategoryId == categoryId)
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetCoursesByDifficultAsync<T>(int difficulty)
        {
            return await this.courseRepository
               .AllAsNoTracking()
               .Where(x => (int)x.Difficulty == difficulty)
               .To<T>()
               .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int courseId)
        {
            var course = await this.courseRepository
                .AllAsNoTracking()
                .Where(x => x.Id == courseId)
                .To<T>()
                .FirstOrDefaultAsync();

            if (course == null)
            {
                throw new NullReferenceException("Doesn't exist this course!");
            }

            var model = this.mapper.Map<T>(course);

            return model;
        }
    }
}
