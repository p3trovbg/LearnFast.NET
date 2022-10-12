namespace LearnFast.Services.Data
{
    using System.Threading.Tasks;

    using AutoMapper;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Web.ViewModels.Course;

    public class CourseService : ICourseService
    {
        private readonly IDeletableEntityRepository<Course> courseRepository;
        private readonly IMapper mapper;

        public CourseService(IMapper mapper, IDeletableEntityRepository<Course> courseRepository)
        {
            this.mapper = mapper;
            this.courseRepository = courseRepository;
        }

        public async Task AddCourse(ImportCourseModel model)
        {
            var newCourse = this.mapper.Map<Course>(model);

            await this.courseRepository.AddAsync(newCourse);
            await this.courseRepository.SaveChangesAsync();
        }
    }
}
