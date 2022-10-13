namespace LearnFast.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISelectorCourse
    {
        Task<IEnumerable<T>> GetFreeCourse<T>();

        Task<IEnumerable<T>> GetOwnCourses<T>(string userId);

        Task<IEnumerable<T>> GetEnrolledCourses<T>(string userId);

        Task<IEnumerable<T>> GetCoursesByLanguage<T>(int languageId);

        Task<IEnumerable<T>> GetCoursesByCategory<T>(int categoryId);

        Task<IEnumerable<T>> GetCoursesByDifficult<T>(int difficulty);
    }
}
