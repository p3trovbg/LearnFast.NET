namespace LearnFast.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Course;

    public interface IFilterCourse
    {
        Task<T> GetByIdAsync<T>(int courseId);

        Task<IEnumerable<T>> GetFreeCourseAsync<T>();

        Task<IEnumerable<T>> GetOwnCoursesAsync<T>(string userId);

        Task<IEnumerable<T>> GetEnrolledCoursesAsync<T>(string userId);

        Task<IEnumerable<T>> GetCoursesByLanguageAsync<T>(int languageId);

        Task<IEnumerable<T>> GetCoursesByCategoryAsync<T>(int categoryId);

        Task<IEnumerable<T>> GetCoursesByDifficultAsync<T>(int difficulty);
    }
}
