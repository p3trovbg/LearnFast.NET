namespace LearnFast.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class CourseService : ICourseService
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

        public async Task AddCourse(ImportCourseModel model)
        {
            var course = this.mapper.Map<Course>(model);

            await this.courseRepository.AddAsync(course);
            await this.courseRepository.SaveChangesAsync();
        }

        public async Task DeleteCourseById(int courseId, string userId)
        {
            var course = this.courseRepository.All()
                .Include(x => x.Owner)
                .FirstOrDefault(x => x.Id == courseId);

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

        public async Task UpdateCourseById(BaseCourseViewModel model, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
