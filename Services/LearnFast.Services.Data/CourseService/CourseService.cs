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
    using LearnFast.Data.Models.Enums;
    using LearnFast.Services.Data.CategoryService;
    using LearnFast.Services.Data.DifficultyService;
    using LearnFast.Services.Data.ImageService;
    using LearnFast.Services.Data.LanguageService;
    using LearnFast.Services.Mapping;
    using LearnFast.Services.Mapping.PropertyMatcher;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Filter;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class CourseService : ICourseService, IFilterCourse
    {
        private const string BaseCourseImageUrl = "https://akm-img-a-in.tosshub.com/indiatoday/images/bodyeditor/202009/e-learning_digital_education-1200x1080.jpg?XjMNHsb4gLoU_cC7110HB7jVghJQROOj";

        private readonly IMapper mapper;
        private readonly IImageService imageService;
        private readonly ICategoryService categoryService;
        private readonly ILanguageService languageService;
        private readonly IDifficultyService difficultyService;
        private readonly IDeletableEntityRepository<Course> courseRepository;

        public CourseService(
            IMapper mapper,
            IDeletableEntityRepository<Course> courseRepository,
            IImageService imageService,
            ICategoryService categoryService,
            ILanguageService languageService,
            IDifficultyService difficultyService)
        {
            this.mapper = mapper;
            this.courseRepository = courseRepository;
            this.imageService = imageService;
            this.categoryService = categoryService;
            this.languageService = languageService;
            this.difficultyService = difficultyService;
        }

        public async Task<int> AddCourseAsync(ImportCourseModel model)
        {
            var course = this.mapper.Map<Course>(model);

            if (model.MainImage != null)
            {
                var image = await this.imageService.UploadImage(model.MainImage, GlobalConstants.ImagesFolderName);
                course.MainImageUrl = image.UrlPath;
            }
            else
            {
                course.MainImageUrl = BaseCourseImageUrl;
            }

            _ = course.Price == 0 ? course.IsFree = true : course.IsFree = false;

            await this.courseRepository.AddAsync(course);
            await this.courseRepository.SaveChangesAsync();

            return course.Id;
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
                throw new NullReferenceException(GlobalExceptions.CourseDoesNotExistExceptionMessage);
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
                throw new NullReferenceException(GlobalExceptions.CourseDoesNotExistExceptionMessage);
            }

            if (userId != course.Owner.Id)
            {
                throw new ArgumentException(GlobalExceptions.DoesNotOwnThisCourseExceptionMessage);
            }

            PropertyCopier<ImportCourseModel, Course>.CopyPropertiesFrom(model, course);

            if (model.MainImage != null)
            {
                var image = await this.imageService.UploadImage(model.MainImage, GlobalConstants.ImagesFolderName);
                course.MainImageUrl = image.UrlPath;
            }

            _ = course.Price == 0 ? course.IsFree = true : course.IsFree = false;
            course.Difficulty = (Difficulty)model.Difficulty;
            await this.courseRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.courseRepository
               .AllAsNoTracking()
               .To<T>()
               .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetOwnCoursesAsync<T>(string userId)
        {
            return await this.courseRepository
               .AllAsNoTracking()
               .Where(x => x.Owner.Id == userId)
               .To<T>()
               .ToListAsync();
        }

        public bool IsUserEnrolledCourse(string userId, int courseId)
        {
            return this.courseRepository.AllAsNoTracking()
                .Any(x => x.CourseStudents.Any(x => x.UserId == userId && x.CourseId == courseId));
        }

        public async Task<T> GetCourseByIdAsync<T>(int courseId)
        {
            var course = await this.courseRepository
                .AllAsNoTracking()
                .Where(x => x.Id == courseId)
                .To<T>()
                .FirstOrDefaultAsync();

            if (course == null)
            {
                throw new NullReferenceException(GlobalExceptions.CourseDoesNotExistExceptionMessage);
            }

            return course;
        }

        public IQueryable<T> GetAllAsQueryAble<T>()
        {
            return this.courseRepository
               .AllAsNoTracking().To<T>();
        }

        public async Task<string> GetOwnerIdByCourse(int courseId)
        {
            var course = await this.courseRepository
                .AllAsNoTracking()
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
            {
                throw new ArgumentException(GlobalExceptions.CourseDoesNotExistExceptionMessage);
            }

            return course.Owner.Id;
        }

        public async Task EnrollCourse(int courseId, string userId)
        {
            var course = await this.courseRepository.All().FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
            {
                throw new NullReferenceException(GlobalExceptions.CourseDoesNotExistExceptionMessage);
            }

            if (this.IsUserEnrolledCourse(userId, courseId))
            {
                throw new ArgumentException(GlobalExceptions.UserAlreadyHasEnrolledInCourse);
            }

            course.CourseStudents.Add(new StudentCourse { CourseId = courseId, UserId = userId });
            await this.courseRepository.SaveChangesAsync();
        }

        public async Task SearchCourses(SearchViewModel model)
        {
            var coursesAsQuery = this.GetAllAsQueryAble<BaseCourseViewModel>();

            if (model.CategoryId != null)
            {
                coursesAsQuery = coursesAsQuery.Where(x => x.Category.Id == model.CategoryId);

                if (!coursesAsQuery.Any())
                {
                    throw new NullReferenceException();
                }
                else
                {
                    model.CategoryName = this.categoryService.GetCategoryName(model.CategoryId);
                }
            }

            coursesAsQuery = this.Filter(model, coursesAsQuery);
            coursesAsQuery = this.Sorter(model.SorterArgument, coursesAsQuery);

            model.Courses = await coursesAsQuery.ToListAsync();

            if (model.Difficulty != null)
            {
                var diffString = Enum.GetName(typeof(Difficulty), model.Difficulty);
                model.Courses = model.Courses.Where(x => x.Difficulty == diffString);
            }

            Pagination(model);

            this.GetDefaultModelProps(model);
        }

        public static void Pagination(SearchViewModel model) 
        {
            model.Page = model.Page == null ? 1 : model.Page;
            model.TotalCount = model.Courses.Count();
            model.Courses = model.Courses
                .Skip((int)((model.Page - 1) * model.ItemsPerPage))
                .Take(model.ItemsPerPage);
        }

        public async Task<IEnumerable<T>> GetTop12BestSellersCourses<T>()
        {
            return await this.courseRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.Sells)
                .Take(12)
                .To<T>()
                .ToListAsync();
        }

        public IQueryable<BaseCourseViewModel> Filter(SearchViewModel model, IQueryable<BaseCourseViewModel> coursesAsQuery)
        {
            if (!string.IsNullOrEmpty(model.SearchString))
            {
                coursesAsQuery = coursesAsQuery.Where(x => x.Title.ToLower().Contains(model.SearchString.ToLower()));
            }

            if (model.InitialPrice >= 0 && model.FinalPrice > 0 && model.IsFree == false)
            {
                coursesAsQuery = coursesAsQuery.Where(x => x.Price >= model.InitialPrice && x.Price <= model.FinalPrice);
            }

            if (model.IsFree)
            {
                coursesAsQuery = coursesAsQuery.Where(x => x.IsFree);
            }

            if (model.LanguageId != null)
            {
                coursesAsQuery = coursesAsQuery.Where(x => x.Language.Id == model.LanguageId);
            }

            return coursesAsQuery;
        }

        public IQueryable<BaseCourseViewModel> Sorter(string sorterAgument, IQueryable<BaseCourseViewModel> coursesAsQuery)
        {
            if (string.IsNullOrEmpty(sorterAgument))
            {
                return coursesAsQuery;
            }

            switch (sorterAgument)
            {
                case GlobalConstants.OrderByTitle:
                    coursesAsQuery = coursesAsQuery.OrderBy(x => x.Title);
                    break;
                case GlobalConstants.OrderByPrice:
                    coursesAsQuery = coursesAsQuery.OrderBy(x => x.Price);
                    break;
                case GlobalConstants.OrderDescByPrice:
                    coursesAsQuery = coursesAsQuery.OrderByDescending(x => x.Price);
                    break;
                case GlobalConstants.OrderByNewestDate:
                    coursesAsQuery = coursesAsQuery.OrderByDescending(x => x.CreatedOn);
                    break;
                case GlobalConstants.OrderByOldestDate:
                    coursesAsQuery = coursesAsQuery.OrderBy(x => x.CreatedOn);
                    break;
            }

            return coursesAsQuery;
        }

        private void GetDefaultModelProps(SearchViewModel model)
        {
            model.Sorter = new List<SelectListItem>
            {
                new SelectListItem { Text = "Order by price", Value = GlobalConstants.OrderByPrice },
                new SelectListItem { Text = "Order by desc price", Value = GlobalConstants.OrderDescByPrice },
                new SelectListItem { Text = "Order by name", Value = GlobalConstants.OrderByTitle },
                new SelectListItem { Text = "The newest", Value = GlobalConstants.OrderByNewestDate },
                new SelectListItem { Text = "The oldest", Value = GlobalConstants.OrderByOldestDate },
            };
        }

    }
}
