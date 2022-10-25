namespace LearnFast.Services.Data.CourseService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Course;

    public interface IFilterCourse
    {
        Task<T> GetByIdAsync<T>(int courseId);

        Task<IEnumerable<T>> GetOwnCoursesAsync<T>(string userId);

        Task<IEnumerable<T>> GetEnrolledCoursesAsync<T>(string userId);
    }
}